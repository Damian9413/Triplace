using Triplace.Domain.Ids;

namespace Triplace.Domain.Entities;

public class CatalogEntry
{
    public CatalogEntryId Id { get; }
    public AttractionId AttractionId { get; }
    public string SnapshotName { get; }
    public bool IsActive { get; private set; }
    public DateTime PublishedAt { get; }

    internal CatalogEntry(CatalogEntryId id, AttractionId attractionId, string snapshotName, DateTime publishedAt)
    {
        Id = id;
        AttractionId = attractionId;
        SnapshotName = snapshotName;
        IsActive = true;
        PublishedAt = publishedAt;
    }

    internal void Deactivate() => IsActive = false;
}
