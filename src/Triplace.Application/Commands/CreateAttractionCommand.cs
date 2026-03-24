using Triplace.Domain.ValueObjects;

namespace Triplace.Application.Commands;

public record CreateAttractionCommand(string Name, IReadOnlyList<MetadataEntry> MetadataEntries);
