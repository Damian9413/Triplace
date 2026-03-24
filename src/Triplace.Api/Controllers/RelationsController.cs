using Microsoft.AspNetCore.Mvc;
using Triplace.Api.DTOs.Requests;
using Triplace.Api.DTOs.Responses;
using Triplace.Api.Mapping;
using Triplace.Application.Services;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;

namespace Triplace.Api.Controllers;

[ApiController]
[Route("api/relations")]
public class RelationsController(RelationService service) : ControllerBase
{
    [HttpPost("exclusion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddExclusion([FromBody] AddRelationRequest request)
    {
        await service.AddExclusionAsync(new AttractionId(request.AttractionIdA), new AttractionId(request.AttractionIdB));
        return NoContent();
    }

    [HttpPost("recommendation")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddRecommendation([FromBody] AddRelationRequest request)
    {
        await service.AddRecommendationAsync(new AttractionId(request.AttractionIdA), new AttractionId(request.AttractionIdB));
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove([FromBody] RemoveRelationRequest request)
    {
        var type = Enum.Parse<AttractionRelationType>(request.Type, true);
        await service.RemoveAsync(
            new AttractionId(request.AttractionIdA),
            new AttractionId(request.AttractionIdB),
            type);
        return NoContent();
    }

    [HttpGet("{attractionId:guid}")]
    [ProducesResponseType(typeof(List<AttractionRelationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelations(Guid attractionId)
    {
        var relations = await service.GetRelationsForAsync(new AttractionId(attractionId));
        return Ok(relations.Select(DomainMapper.ToResponse).ToList());
    }

    [HttpGet("check-exclusive")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckExclusive([FromQuery] Guid a, [FromQuery] Guid b)
    {
        var result = await service.AreExclusiveAsync(new AttractionId(a), new AttractionId(b));
        return Ok(result);
    }
}
