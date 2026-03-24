using Triplace.Domain.Ids;

namespace Triplace.Application.Commands;

public record AddGroupEntryCommand(
    AttractionGroupId GroupId,
    Guid NodeId,
    string NodeType,
    IReadOnlyList<AddAddonCommand>? Addons = null);

public record AddAddonCommand(Guid AddonTypeId, Dictionary<string, object> Values);
