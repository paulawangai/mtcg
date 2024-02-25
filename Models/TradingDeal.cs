using System;
using System.ComponentModel.DataAnnotations;

public enum DealType
{
    Monster,
    Spell
}

public class TradingDeal
{
    [Key]
    public Guid Id { get; set; }

    
    public Guid CardToTrade { get; set; }

    
    public DealType? Type { get; set; }

    public double MinimumDamage { get; set; }

    public string CreatorUsername { get; set; }
}

