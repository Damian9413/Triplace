using Microsoft.AspNetCore.Mvc;
using Triplace.Api.DTOs.Requests;
using Triplace.Api.DTOs.Responses;
using Triplace.Api.Mapping;
using Triplace.Application.Commands;
using Triplace.Application.Services;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Api.Controllers;

[ApiController]
[Route("api/catalogs")]
public class SeasonalCatalogsController(SeasonalCatalogService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateSeasonalCatalogRequest request)
    {
        var command = new CreateSeasonalCatalogCommand(
            request.Name,
            Enum.Parse<Season>(request.Season, true),
            DateOnly.Parse(request.ValidFrom),
            DateOnly.Parse(request.ValidTo),
            request.Region,
            request.Description,
            request.MaxCapacity);

        var id = await service.CreateAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = id.Value }, new { id = id.Value });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<SeasonalCatalogResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var catalogs = await service.GetAllAsync();
        return Ok(catalogs.Select(DomainMapper.ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SeasonalCatalogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var catalog = await service.GetByIdAsync(new SeasonalCatalogId(id));
        if (catalog is null) return NotFound();
        return Ok(DomainMapper.ToResponse(catalog));
    }

    [HttpGet("season/{season}")]
    [ProducesResponseType(typeof(List<SeasonalCatalogResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySeason(string season)
    {
        var s = Enum.Parse<Season>(season, true);
        var catalogs = await service.GetBySeasonAsync(s);
        return Ok(catalogs.Select(DomainMapper.ToResponse).ToList());
    }

    [HttpPost("{id:guid}/attractions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddAttraction(Guid id, [FromBody] PublishToCatalogRequest request)
    {
        await service.AddAttractionAsync(
            new SeasonalCatalogId(id),
            new AttractionId(request.AttractionId));
        return NoContent();
    }

    [HttpDelete("{id:guid}/attractions/{attractionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveAttraction(Guid id, Guid attractionId)
    {
        await service.RemoveAttractionAsync(new SeasonalCatalogId(id), new AttractionId(attractionId));
        return NoContent();
    }
}
