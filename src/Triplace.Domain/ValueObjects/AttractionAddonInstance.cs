using Triplace.Domain.Entities;

namespace Triplace.Domain.ValueObjects;

public class AttractionAddonInstance
{
    public AttractionAddonType Type { get; }
    public IReadOnlyDictionary<string, object> Values { get; }

    internal AttractionAddonInstance(AttractionAddonType type, Dictionary<string, object> values)
    {
        Type = type;
        Values = new Dictionary<string, object>(values);
    }
}
