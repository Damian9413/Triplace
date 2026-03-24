using Microsoft.AspNetCore.Mvc;
using Triplace.Api.DTOs.Requests;
using Triplace.Api.DTOs.Responses;
using Triplace.Api.Mapping;
using Triplace.Application.Commands;
using Triplace.Application.Services;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Api.Controllers;

[ApiController]
[Route("api/addon-types")]
public class AddonTypesController(AttractionAddonTypeService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAddonTypeRequest request)
    {
        var fields = request.Fields.Select(f => new FieldDefinitionCommand(
            f.FieldName,
            Enum.Parse<FieldValueType>(f.ValueType, true),
            BuildConstraint(f)
        )).ToList();

        var command = new CreateAddonTypeCommand(request.Name, fields);
        var id = await service.CreateAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = id.Value }, new { id = id.Value });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<AddonTypeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var types = await service.GetAllAsync();
        return Ok(types.Select(DomainMapper.ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AddonTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var addonType = await service.GetByIdAsync(new AttractionAddonTypeId(id));
        if (addonType is null) return NotFound();
        return Ok(DomainMapper.ToResponse(addonType));
    }

    private static FieldConstraint BuildConstraint(FieldDefinitionRequest f)
    {
        if (f.ConstraintType.Equals("AllowedValues", StringComparison.OrdinalIgnoreCase)
            && f.AllowedValues is { Length: > 0 })
            return new AllowedValuesConstraint(f.AllowedValues);

        if (f.ConstraintType.Equals("DateRange", StringComparison.OrdinalIgnoreCase)
            && f.DateFrom is not null && f.DateTo is not null)
            return new DateRangeConstraint(
                DateOnly.Parse(f.DateFrom),
                DateOnly.Parse(f.DateTo));

        return new UnconstrainedConstraint();
    }
}
