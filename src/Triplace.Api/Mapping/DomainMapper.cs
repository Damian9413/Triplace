using Triplace.Api.DTOs.Responses;
using Triplace.Domain.Entities;
using Triplace.Domain.ValueObjects;

namespace Triplace.Api.Mapping;

public static class DomainMapper
{
    public static AttractionResponse ToResponse(Attraction a) => new()
    {
        Id = a.Id.Value,
        Name = a.Name,
        Status = a.Status.ToString(),
        Category = a.Category.ToString(),
        BestSeasons = a.BestSeasons.Select(s => s.ToString()).ToList(),
        Duration = a.Duration.ToString(),
        IsOutdoor = a.IsOutdoor,
        IsFree = a.IsFree,
        Amenities = a.Amenities.Select(am => am.ToString()).ToList(),
        ParentId = a.ParentId?.Value,
        IsLeaf = a.IsLeaf,
        Children = a.Children.Select(ToResponse).ToList(),
        Metadata = new AttractionMetadataResponse
        {
            Entries = a.Metadata.Entries.Select(e => new MetadataEntryResponse
            {
                Label = e.Label,
                Value = e.Value
            }).ToList()
        }
    };

    public static SeasonalCatalogResponse ToResponse(SeasonalCatalog c) => new()
    {
        Id = c.Id.Value,
        Name = c.Name,
        Metadata = new CatalogMetadataResponse
        {
            Season = c.Metadata.Season.ToString(),
            ValidFrom = c.Metadata.ValidFrom.ToString("yyyy-MM-dd"),
            ValidTo = c.Metadata.ValidTo.ToString("yyyy-MM-dd"),
            Region = c.Metadata.Region,
            Description = c.Metadata.Description,
            MaxCapacity = c.Metadata.MaxCapacity
        },
        AttractionIds = c.Attractions.Select(id => id.Value).ToList()
    };

    public static RouteResponse ToResponse(Domain.Entities.Route r) => new()
    {
        Id = r.Id.Value,
        Name = r.Name,
        Description = r.Description,
        Season = r.Season.ToString(),
        ScopeAttractionId = r.ScopeAttractionId?.Value,
        MustHaves = r.GetMustHaves().Select(ToResponse).ToList(),
        Optionals = r.GetOptionals().Select(ToResponse).ToList()
    };

    public static RouteItemResponse ToResponse(RouteItem i) => new()
    {
        Id = i.Id.Value,
        AttractionId = i.AttractionId.Value,
        Priority = i.Priority.ToString(),
        SortOrder = i.SortOrder
    };

    public static AttractionRelationResponse ToResponse(AttractionRelation r) => new()
    {
        LowId = r.LowId.Value,
        HighId = r.HighId.Value,
        Type = r.Type.ToString()
    };
}
