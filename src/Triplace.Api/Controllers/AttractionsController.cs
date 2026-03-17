using Microsoft.AspNetCore.Mvc;
using Triplace.Application.Attractions;

namespace Triplace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttractionsController(GetAttractionsByCity getAttractionsByCity) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByCity(
        [FromQuery] string city,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest("Parametr 'city' jest wymagany.");

        var attractions = await getAttractionsByCity.ExecuteAsync(city, cancellationToken);
        return Ok(attractions);
    }
}
