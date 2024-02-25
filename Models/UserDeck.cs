using System;
using System.Collections.Generic;

public class UserDeck
{
    public Guid Id { get; set; }
    public List<Card> Cards { get; set; } // List of cards in the deck

    public UserDeck()
    {
        Id = Guid.NewGuid(); // Generate a new unique ID for each deck
        Cards = new List<Card>();
    }
}