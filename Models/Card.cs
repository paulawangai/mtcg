using System.Text.Json.Serialization;

public class Card
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? Damage { get; set; }
    public ElementType? ElementType { get; set; }
    public CardType? Type { get; set; }

    // Foreign key to associate the card with a package
    public Guid? PackageId { get; set; }
    public Guid? OwnerId { get; set; }

    public Guid? UserDeckId { get; set; } // Foreign key

    [JsonIgnore]
    public UserDeck UserDeck { get; set; } // Navigation property
}

public enum ElementType
{
    Fire,
    Water,
    Normal
}

public enum CardType
{
    Monster,
    Spell
}
