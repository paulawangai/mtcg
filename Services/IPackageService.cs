using System.Collections.Generic;

public interface IPackageService
{
    bool CardsExist(List<Card> cards);
    void SavePackage(List<Card> cards, Guid? ownerId);
}


