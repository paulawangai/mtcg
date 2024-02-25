using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace mtcg.Services
{
    public class BattleService : IBattleService
    {
        private readonly mtcgDbContext _context;

        // Constructor injection of mtcgDbContext
        public BattleService(mtcgDbContext context)
        {
            _context = context;
        }

        public async Task<BattleResult> RequestBattle(Guid player1Id, Guid player2Id)
        {
            // Use async methods for database operations
            var player1Deck = await GetDeckByUserIdAsync(player1Id);
            var player2Deck = await GetDeckByUserIdAsync(player2Id);

            // Validate decks
            if (player1Deck == null || !player1Deck.Any())
                throw new InvalidOperationException($"Player {player1Id} does not have a valid deck.");
            if (player2Deck == null || !player2Deck.Any())
                throw new InvalidOperationException($"Player {player2Id} does not have a valid deck.");

            // Create a new battle
            var battle = new Battle
            {
                Id = Guid.NewGuid(),
                Player1Id = player1Id,
                Player2Id = player2Id,
                Rounds = new List<Round>(),
                Result = null
            };

            // Start battle
            int roundCounter = 1;
            int player1RoundWins = 0;
            int player2RoundWins = 0;

            // Run exactly 4 rounds
            while (roundCounter <= 4)
            {
                // Retrieve a random card from each player's deck
                var player1Card = GetRandomCardFromDeck(player1Deck);
                var player2Card = GetRandomCardFromDeck(player2Deck);

                // Conduct round
                var roundResult = ConductRound(player1Card, player2Card);

                // Log round
                battle.Rounds.Add(new Round
                {
                    RoundId = Guid.NewGuid(),
                    BattleId = battle.Id,
                    Player1 = _context.Users.FirstOrDefault(u => u.UserId == player1Id),
                    Player2 = _context.Users.FirstOrDefault(u => u.UserId == player2Id),
                    Player1Card = player1Card,
                    Player2Card = player2Card,
                    Result = roundResult
                });

                // Check round result and update round wins
                if (roundResult == RoundResult.Player1Wins)
                {
                    player1RoundWins++;
                }
                else if (roundResult == RoundResult.Player2Wins)
                {
                    player2RoundWins++;
                }

                // Increment round counter
                roundCounter++;
            }

            // Determine the winner based on round wins
            Guid winnerId, loserId;
            bool isDraw = false;
            if (player1RoundWins > player2RoundWins)
            {
                winnerId = player1Id;
                
            }
            else if (player2RoundWins > player1RoundWins)
            {
                winnerId = player2Id;
                
            }
            else
            {
                // It's a draw if both players have the same number of round wins
                isDraw = true;
                winnerId = Guid.Empty;
                loserId = Guid.Empty;
            }

            // Update battle result
            battle.Result = new BattleResult
            {
                BattleResultId = Guid.NewGuid(),
                WinnerId = winnerId,
                IsDraw = isDraw,
                
            };

            // Update winner and loser stats if it's not a draw
            if (!isDraw)
            {
                UpdateUserStats(winnerId);
            }

            return battle.Result;
        }


        private void UpdateUserStats(Guid winnerId)
        {
            // Retrieve UserStats objects for the winner and loser
            var winnerStats = _context.UserStats.FirstOrDefault(us => us.UserId == winnerId);

            if (winnerStats != null)
            {
                // Update winner stats
                winnerStats.Wins++;
                winnerStats.Elo += 10; // Adjust Elo as needed

                
                // Save changes to database
                _context.SaveChanges();
            }
        }



        private async Task<List<Card>> GetDeckByUserIdAsync(Guid playerId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == playerId);
            if (user != null && user.ConfiguredDeck != null)
                return user.ConfiguredDeck.Cards.Take(4).ToList();
            return await _context.Cards.Where(c => c.OwnerId == playerId).Take(4).ToListAsync();
        }


        private Card GetRandomCardFromDeck(List<Card> deck)
        {
            var random = new Random();
            return deck[random.Next(deck.Count)];
        }

        private RoundResult ConductRound(Card player1Card, Card player2Card)
        {
            bool isPlayer1Monster = player1Card.Type == CardType.Monster;
            bool isPlayer2Monster = player2Card.Type == CardType.Monster;

            // If both players have monster cards, compare their damage directly
            if (isPlayer1Monster && isPlayer2Monster)
            {
                if (player1Card.Damage > player2Card.Damage)
                    return RoundResult.Player1Wins;
                else if (player2Card.Damage > player1Card.Damage)
                    return RoundResult.Player2Wins;
                else
                    return RoundResult.Draw;
            }
            else
            {
                // If at least one player has a spell card, handle elemental effectiveness
                if (!isPlayer1Monster)
                {
                    double player1Damage = ApplyElementalEffectiveness(player1Card, player2Card);
                    if (player1Damage > player2Card.Damage)
                        return RoundResult.Player1Wins;
                    else if (player1Damage < player2Card.Damage)
                        return RoundResult.Player2Wins;
                    else
                        return RoundResult.Draw;
                }
                else if (!isPlayer2Monster)
                {
                    double player2Damage = ApplyElementalEffectiveness(player2Card, player1Card);
                    if (player2Damage > player1Card.Damage)
                        return RoundResult.Player2Wins;
                    else if (player2Damage < player1Card.Damage)
                        return RoundResult.Player1Wins;
                    else
                        return RoundResult.Draw;
                }
            }

            // Default case: draw
            return RoundResult.Draw;
        }

        private double ApplyElementalEffectiveness(Card spellCard, Card targetCard)
        {
            double damage = spellCard.Damage ?? 0; // Default damage if not specified

            // Check if the spell card's element type is effective against the target card's element type
            if (spellCard.ElementType.HasValue && targetCard.ElementType.HasValue)
            {
                switch (spellCard.ElementType.Value)
                {
                    case ElementType.Fire:
                        if (targetCard.ElementType == ElementType.Normal)
                            damage *= 2; // Double damage against normal type
                        break;
                    case ElementType.Water:
                        if (targetCard.ElementType == ElementType.Fire)
                            damage *= 2; // Double damage against fire type
                        break;
                    case ElementType.Normal:
                        if (targetCard.ElementType == ElementType.Water)
                            damage *= 2; // Double damage against water type
                        break;
                }
            }

            return damage;
        }


    }
}
