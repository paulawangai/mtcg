using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid UserId { get; set; }
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }

    public double Coins { get; set; }

    public UserStats? UserStats { get; set; }

    public Guid? ConfiguredDeckId { get; set; } // Foreign key

    public UserDeck ConfiguredDeck { get; set; } // Navigation property

    public string? Bio { get; set; } // Add Bio property

    public Guid? OpponentId { get; set; } // Store the opponent's user ID

    public Guid? BattleId { get; set; } // Store the battle ID once initiated

}
