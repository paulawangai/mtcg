using System;

public class DeckConfiguration
{
    public List<Guid> CardIds { get; set; } // List of card IDs selected for the deck

    public DeckConfiguration()
    {
        CardIds = new List<Guid>();
    }
}
