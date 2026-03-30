using Microsoft.AspNetCore.Mvc;
using Triplace.Api.DTOs.Requests;
using Triplace.Api.DTOs.Responses;
using Triplace.Api.Mapping;
using Triplace.Application.Commands;
using Triplace.Application.Services;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;
using Triplace.Domain.Specifications;
using Triplace.Domain.ValueObjects;

namespace Triplace.Api.Controllers;

[ApiController]
[Route("api/attractions")]
public class AttractionsController(AttractionService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAttractionRequest request)
    {
        var bestSeasons = request.BestSeasons
            .Select(s => Enum.Parse<Season>(s, true))
            .ToHashSet();

        var amenities = request.Amenities
            .Select(a => Enum.Parse<AttractionAmenity>(a, true))
            .ToHashSet();

        var command = new CreateAttractionCommand(
            request.Name,
            Enum.Parse<AttractionCategory>(request.Category, true),
            bestSeasons,
            Enum.Parse<VisitDuration>(request.Duration, true),
            request.IsOutdoor,
            request.IsFree,
            amenities,
            request.Metadata.Select(m => new MetadataEntry(m.Label, m.Value)).ToList());

        var id = await service.CreateDraftAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = id.Value }, new { id = id.Value });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<AttractionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var attractions = await service.GetAllAsync();
        return Ok(attractions.Select(DomainMapper.ToResponse).ToList());
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<AttractionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string? category,
        [FromQuery] string? season,
        [FromQuery] bool? isOutdoor,
        [FromQuery] bool? isFree,
        [FromQuery] string? duration,
        [FromQuery] string? amenity)
    {
        var specs = new List<ISpecification<Domain.Entities.Attraction>>();

        if (category is not null)
            specs.Add(new InCategorySpec(Enum.Parse<AttractionCategory>(category, true)));
        if (season is not null)
            specs.Add(new ForSeasonSpec(Enum.Parse<Season>(season, true)));
        if (duration is not null)
            specs.Add(new WithDurationSpec(Enum.Parse<VisitDuration>(duration, true)));
        if (isOutdoor is true)
            specs.Add(new IsOutdoorSpec());
        if (isFree is true)
            specs.Add(new IsFreeSpec());
        if (amenity is not null)
            specs.Add(new HasAmenitySpec(Enum.Parse<AttractionAmenity>(amenity, true)));

        if (specs.Count == 0)
        {
            var all = await service.GetAllAsync();
            return Ok(all.Select(DomainMapper.ToResponse).ToList());
        }

        var combined = specs.Aggregate((a, b) => a.And(b));
        var results = await service.FindBySpecAsync(combined);
        return Ok(results.Select(DomainMapper.ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AttractionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var attraction = await service.GetByIdAsync(new AttractionId(id));
        if (attraction is null) return NotFound();
        return Ok(DomainMapper.ToResponse(attraction));
    }

    [HttpPost("{id:guid}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Publish(Guid id)
    {
        await service.PublishAsync(new AttractionId(id));
        return NoContent();
    }

    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Archive(Guid id)
    {
        await service.ArchiveAsync(new AttractionId(id));
        return NoContent();
    }

    [HttpPost("{parentId:guid}/children/{childId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddChild(Guid parentId, Guid childId)
    {
        await service.AddChildAsync(new AttractionId(parentId), new AttractionId(childId));
        return NoContent();
    }
}
