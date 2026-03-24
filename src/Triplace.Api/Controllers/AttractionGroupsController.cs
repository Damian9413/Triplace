using Microsoft.AspNetCore.Mvc;
using Triplace.Api.DTOs.Requests;
using Triplace.Api.DTOs.Responses;
using Triplace.Api.Mapping;
using Triplace.Application.Commands;
using Triplace.Application.Services;
using Triplace.Domain.Ids;

namespace Triplace.Api.Controllers;

[ApiController]
[Route("api/groups")]
public class AttractionGroupsController(AttractionGroupService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAttractionGroupRequest request)
    {
        var id = await service.CreateAsync(new CreateAttractionGroupCommand(request.Name));
        return CreatedAtAction(nameof(GetById), new { id = id.Value }, new { id = id.Value });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AttractionGroupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var group = await service.GetByIdAsync(new AttractionGroupId(id));
        if (group is null) return NotFound();
        return Ok(DomainMapper.ToResponse(group));
    }

    [HttpGet("{id:guid}/flatten")]
    [ProducesResponseType(typeof(List<AttractionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Flatten(Guid id)
    {
        var attractions = await service.FlattenAsync(new AttractionGroupId(id));
        return Ok(attractions.Select(DomainMapper.ToResponse).ToList());
    }

    [HttpPost("{id:guid}/entries")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddEntry(Guid id, [FromBody] AddEntryRequest request)
    {
        var addons = request.Addons?.Select(a => new AddAddonCommand(a.AddonTypeId, a.Values)).ToList();
        var command = new AddGroupEntryCommand(
            new AttractionGroupId(id),
            request.NodeId,
            request.NodeType,
            addons);

        await service.AddEntryAsync(command);
        return NoContent();
    }
}
