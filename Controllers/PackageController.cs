using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using mtcg;

[ApiController]
[Route("packages")]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _packageService;

    public PackagesController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdmin")] // Require admin to access this endpoint
    public IActionResult CreatePackage(List<Card> cards)

    {
        if (cards.Count != 5)
        {
            return BadRequest("A package must contain exactly 5 cards");
        }

        if (_packageService.CardsExist(cards))
        {
            return Conflict("At least one card in the package already exists");
        }

        Guid? ownerId = null;
        _packageService.SavePackage(cards, ownerId);

        return Created("Package and cards successfully created", null);
    }
}
