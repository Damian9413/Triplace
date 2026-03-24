namespace Triplace.Api.DTOs.Responses;

public class AttractionGroupResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<AttractionEntryResponse> Entries { get; set; } = [];
}

public class AttractionEntryResponse
{
    public Guid Id { get; set; }
    public string NodeType { get; set; } = string.Empty;
    public Guid NodeId { get; set; }
    public string NodeName { get; set; } = string.Empty;
    public List<AttractionEntryResponse>? NestedEntries { get; set; }
    public List<AddonInstanceResponse> Addons { get; set; } = [];
}

public class AddonInstanceResponse
{
    public Guid AddonTypeId { get; set; }
    public string AddonTypeName { get; set; } = string.Empty;
    public Dictionary<string, object> Values { get; set; } = [];
}
