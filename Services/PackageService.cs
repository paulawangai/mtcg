using System;
using System.Collections.Generic;
using System.Linq;
using mtcg;

public class PackageService : IPackageService
{
    private readonly mtcgDbContext _context;

    public PackageService(mtcgDbContext context)
    {
        _context = context;
    }

    public bool HasPackages()
    {
        return _context.Packages.Any();
    }

    public bool CardsExist(List<Card> cards)
    {
        foreach (var card in cards)
        {
            // Check if any card with the same ID already exists in the database
            if (_context.Cards.Any(c => c.Id == card.Id))
            {
                return true; // At least one card already exists
            }
        }
        return false; // No cards exist in the database
    }

    public void SavePackage(List<Card> cards, Guid? ownerId)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                // Create a new package entity
                var newPackage = new Package { OwnerId = ownerId };


                // Add the package to the context
                _context.Packages.Add(newPackage);

                // Save changes to generate the package ID
                _context.SaveChanges();

                // Associate each card with the newly created package
                foreach (var card in cards)
                {
                    // Set card type and element type based on the card name
                    card.Type = GetCardTypeFromName(card.Name);
                    card.ElementType = GetElementTypeFromName(card.Name);

                    // Associate the card with the package
                    card.PackageId = newPackage.Id;
                }

                // Add all cards to the context at once
                _context.Cards.AddRange(cards);

                // Save changes to both the Packages and Cards tables
                _context.SaveChanges();

                transaction.Commit(); // Commit the transaction if successful
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Rollback the transaction if an error occurs
                throw new Exception("Failed to save package. See inner exception for details.", ex);
            }
        }
    }


    private CardType GetCardTypeFromName(string name)
    {
        return name.Contains("Spell", StringComparison.OrdinalIgnoreCase) ? CardType.Spell : CardType.Monster;
    }

    private ElementType GetElementTypeFromName(string name)
    {
        if (name.Contains("WaterGoblin", StringComparison.OrdinalIgnoreCase))
        {
            return ElementType.Water;
        }
        else if (name.Contains("Dragon", StringComparison.OrdinalIgnoreCase))
        {
            return ElementType.Fire;
        }
        else if (name.Contains("Ork", StringComparison.OrdinalIgnoreCase))
        {
            return ElementType.Normal;
        }
        else if (name.Contains("Water", StringComparison.OrdinalIgnoreCase))
        {
            return ElementType.Water;
        }
        else if (name.Contains("Fire", StringComparison.OrdinalIgnoreCase))
        {
            return ElementType.Fire;
        }
        else
        {
            return ElementType.Normal;
        }
    }
}
