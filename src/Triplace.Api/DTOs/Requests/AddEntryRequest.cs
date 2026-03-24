namespace Triplace.Api.DTOs.Requests;

public class AddEntryRequest
{
    public Guid NodeId { get; set; }
    public string NodeType { get; set; } = string.Empty; // "attraction" or "group"
    public List<AddAddonRequest>? Addons { get; set; }
}

public class AddAddonRequest
{
    public Guid AddonTypeId { get; set; }
    public Dictionary<string, object> Values { get; set; } = [];
}
