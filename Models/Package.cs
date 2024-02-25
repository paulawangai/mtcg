using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Package
{
    [Key]
    public Guid Id { get; set; }

    public List<Card> Cards { get; set; }

    public double Price { get; set; }

    public Guid? OwnerId { get; set; } // Nullable Guid to store the owner's Id

    // Constructor to set the default price to 5.0
    public Package()
    {
        Price = 5.0;
        OwnerId = null;
    }
}
