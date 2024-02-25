public class BattleResult
{
    public Guid BattleResultId { get; set; } // Primary key
    public Guid WinnerId { get; set; } // Foreign key to UserStats
    public bool IsDraw { get; set; }

    

    // Additional properties and methods
}
