using System;
using System.Collections.Generic;

public class Battle
{
    public Guid Id { get; set; }
    public Guid Player1Id { get; set; } // Updated data type to string
    public Guid Player2Id { get; set; } // Updated data type to string
    public BattleStatus Status { get; set; } // Added Status property
    public BattleResult Result { get; set; }
    public List<Round> Rounds { get; set; }
}

public enum BattleStatus
{
    Initiated,
    Accepted,
    Completed
}
