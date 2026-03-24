namespace Triplace.Api.DTOs.Requests;

public class CreateAddonTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public List<FieldDefinitionRequest> Fields { get; set; } = [];
}

public class FieldDefinitionRequest
{
    public string FieldName { get; set; } = string.Empty;
    public string ValueType { get; set; } = "String"; // String, Boolean, Int, Date
    public string ConstraintType { get; set; } = "Unconstrained"; // Unconstrained, AllowedValues, DateRange
    public string[]? AllowedValues { get; set; }
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
}
