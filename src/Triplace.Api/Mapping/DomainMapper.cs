using Triplace.Api.DTOs.Responses;
using Triplace.Domain.Entities;
using Triplace.Domain.Interfaces;
using Triplace.Domain.ValueObjects;

namespace Triplace.Api.Mapping;

public static class DomainMapper
{
    public static AttractionResponse ToResponse(Attraction a) => new()
    {
        Id = a.Id.Value,
        Name = a.Name,
        Status = a.Status.ToString(),
        Metadata = new AttractionMetadataResponse
        {
            Entries = a.Metadata.Entries.Select(e => new MetadataEntryResponse
            {
                Label = e.Label,
                Value = e.Value
            }).ToList()
        }
    };

    public static AttractionGroupResponse ToResponse(AttractionGroup g) => new()
    {
        Id = g.Id.Value,
        Name = g.Name,
        Entries = g.Entries.Select(ToResponse).ToList()
    };

    public static AttractionEntryResponse ToResponse(AttractionEntry e)
    {
        var response = new AttractionEntryResponse
        {
            Id = e.Id.Value,
            Addons = e.Addons.Select(ToResponse).ToList()
        };

        if (e.Node is Attraction attraction)
        {
            response.NodeType = "Attraction";
            response.NodeId = attraction.Id.Value;
            response.NodeName = attraction.Name;
        }
        else if (e.Node is AttractionGroup group)
        {
            response.NodeType = "Group";
            response.NodeId = group.Id.Value;
            response.NodeName = group.Name;
            response.NestedEntries = group.Entries.Select(ToResponse).ToList();
        }

        return response;
    }

    public static AddonInstanceResponse ToResponse(AttractionAddonInstance addon) => new()
    {
        AddonTypeId = addon.Type.Id.Value,
        AddonTypeName = addon.Type.Name,
        Values = new Dictionary<string, object>(addon.Values)
    };

    public static AddonTypeResponse ToResponse(AttractionAddonType t) => new()
    {
        Id = t.Id.Value,
        Name = t.Name,
        Fields = t.Fields.Select(ToResponse).ToList()
    };

    public static FieldDefinitionResponse ToResponse(AddonFieldDefinition f)
    {
        var response = new FieldDefinitionResponse
        {
            FieldName = f.FieldName,
            ValueType = f.ValueType.ToString(),
        };

        switch (f.Constraint)
        {
            case AllowedValuesConstraint avc:
                response.ConstraintType = "AllowedValues";
                response.AllowedValues = avc.AllowedValues;
                break;
            case DateRangeConstraint:
                response.ConstraintType = "DateRange";
                break;
            default:
                response.ConstraintType = "Unconstrained";
                break;
        }

        return response;
    }

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
        Entries = c.Entries.Select(ToResponse).ToList()
    };

    public static CatalogEntryResponse ToResponse(CatalogEntry e) => new()
    {
        Id = e.Id.Value,
        AttractionId = e.AttractionId.Value,
        SnapshotName = e.SnapshotName,
        IsActive = e.IsActive,
        PublishedAt = e.PublishedAt
    };

    public static RouteResponse ToResponse(Domain.Entities.Route r) => new()
    {
        Id = r.Id.Value,
        Name = r.Name,
        Description = r.Description,
        Season = r.Season.ToString(),
        ScopeGroupId = r.ScopeGroupId?.Value,
        MustHaves = r.GetMustHaves().Select(ToResponse).ToList(),
        Optionals = r.GetOptionals().Select(ToResponse).ToList()
    };

    public static RouteItemResponse ToResponse(RouteItem i) => new()
    {
        Id = i.Id.Value,
        CatalogEntryId = i.CatalogEntryId.Value,
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
