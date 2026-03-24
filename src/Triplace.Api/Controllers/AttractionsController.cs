using Microsoft.AspNetCore.Mvc;
using Triplace.Api.DTOs.Requests;
using Triplace.Api.DTOs.Responses;
using Triplace.Api.Mapping;
using Triplace.Application.Commands;
using Triplace.Application.Services;
using Triplace.Domain.Ids;
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
        var entries = request.Metadata.Select(m => new MetadataEntry(m.Label, m.Value)).ToList();
        var command = new CreateAttractionCommand(request.Name, entries);
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
}
