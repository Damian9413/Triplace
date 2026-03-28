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
[Route("api/routes")]
public class RoutesController(RouteService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<RouteResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var routes = await service.GetAllAsync();
        return Ok(routes.Select(DomainMapper.ToResponse).ToList());
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRouteRequest request)
    {
        var items = request.Items.Select(i => new RouteItemCommand(
            new AttractionId(i.AttractionId),
            Enum.Parse<Priority>(i.Priority, true)
        )).ToList();

        var command = new CreateRouteCommand(
            request.Name,
            request.Description,
            Enum.Parse<Season>(request.Season, true),
            request.ScopeGroupId.HasValue ? new AttractionGroupId(request.ScopeGroupId.Value) : null,
            items);

        var id = await service.CreateAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = id.Value }, new { id = id.Value });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RouteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var route = await service.GetByIdAsync(new RouteId(id));
        if (route is null) return NotFound();
        return Ok(DomainMapper.ToResponse(route));
    }

    [HttpPost("{id:guid}/items")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddItem(Guid id, [FromBody] AddRouteItemRequest request)
    {
        await service.AddItemAsync(
            new RouteId(id),
            new AttractionId(request.AttractionId),
            Enum.Parse<Priority>(request.Priority, true));
        return NoContent();
    }

    [HttpDelete("{id:guid}/items/{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveItem(Guid id, Guid itemId)
    {
        await service.RemoveItemAsync(new RouteId(id), new RouteItemId(itemId));
        return NoContent();
    }

    [HttpPut("{id:guid}/items/{itemId:guid}/reorder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReorderItem(Guid id, Guid itemId, [FromBody] ReorderRequest request)
    {
        await service.ReorderItemAsync(new RouteId(id), new RouteItemId(itemId), request.NewSortOrder);
        return NoContent();
    }

    [HttpGet("{id:guid}/check-exclusions")]
    [ProducesResponseType(typeof(List<ExclusionConflictResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckExclusions(Guid id)
    {
        var conflicts = await service.CheckExclusionsAsync(new RouteId(id));
        return Ok(conflicts.Select(c => new ExclusionConflictResponse
        {
            AttractionIdA = c.AttractionIdA.Value,
            AttractionIdB = c.AttractionIdB.Value
        }).ToList());
    }
}
