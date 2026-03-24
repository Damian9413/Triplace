using Triplace.Domain.Entities;

namespace Triplace.Domain.Interfaces;

public interface IAttractionNode
{
    IReadOnlyList<Attraction> Flatten();
}
