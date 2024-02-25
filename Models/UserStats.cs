public class UserStats
{
    public Guid UserId { get; set; } // Primary key and foreign key
    public string? Name { get; set; }
    public int Elo { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }

    // Navigation property
    public User? User { get; set; }
}

