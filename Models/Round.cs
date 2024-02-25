public class Round
{
    public Guid RoundId { get; set; }
    public Guid BattleId { get; set; }
    public User Player1 { get; set; }
    public User Player2 { get; set; }
    public Card Player1Card { get; set; }
    public Card Player2Card { get; set; }
    public RoundResult Result { get; set; }
}
