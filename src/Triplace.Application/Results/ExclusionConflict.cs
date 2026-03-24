using Triplace.Domain.Ids;

namespace Triplace.Application.Results;

public record ExclusionConflict(AttractionId AttractionIdA, AttractionId AttractionIdB);
