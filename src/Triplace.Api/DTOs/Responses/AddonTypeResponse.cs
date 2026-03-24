namespace Triplace.Api.DTOs.Responses;

public class AddonTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<FieldDefinitionResponse> Fields { get; set; } = [];
}

public class FieldDefinitionResponse
{
    public string FieldName { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public string ConstraintType { get; set; } = string.Empty;
    public string[]? AllowedValues { get; set; }
}
