using Triplace.Application.Commands;
using Triplace.Application.Repositories;
using Triplace.Domain.Builders;
using Triplace.Domain.Entities;
using Triplace.Domain.Enums;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;

namespace Triplace.Application.Services;

public class SeasonalCatalogService(
    ISeasonalCatalogRepository catalogRepository,
    IAttractionRepository attractionRepository)
{
    public async Task<SeasonalCatalogId> CreateAsync(CreateSeasonalCatalogCommand command)
    {
        var catalog = SeasonalCatalogBuilder.Create(command.Name)
            .WithMetadata(m =>
            {
                m.ForSeason(command.Season)
                 .ValidFrom(command.ValidFrom)
                 .ValidTo(command.ValidTo)
                 .InRegion(command.Region)
                 .WithDescription(command.Description);

                if (command.MaxCapacity.HasValue)
                    m.WithMaxCapacity(command.MaxCapacity.Value);
            })
            .Build();

        await catalogRepository.SaveAsync(catalog);
        return catalog.Id;
    }

    public async Task AddAttractionAsync(SeasonalCatalogId catalogId, AttractionId attractionId)
    {
        var catalog = await GetCatalogOrThrowAsync(catalogId);
        var attraction = await attractionRepository.GetByIdAsync(attractionId);
        if (attraction is null)
            throw new EntityNotFoundException($"Attraction {attractionId.Value} not found.");

        catalog.AddAttraction(attraction);
        await catalogRepository.SaveAsync(catalog);
    }

    public async Task RemoveAttractionAsync(SeasonalCatalogId catalogId, AttractionId attractionId)
    {
        var catalog = await GetCatalogOrThrowAsync(catalogId);
        catalog.RemoveAttraction(attractionId);
        await catalogRepository.SaveAsync(catalog);
    }

    public Task<SeasonalCatalog?> GetByIdAsync(SeasonalCatalogId id)
        => catalogRepository.GetByIdAsync(id);

    public Task<IReadOnlyList<SeasonalCatalog>> GetAllAsync()
        => catalogRepository.GetAllAsync();

    public Task<IReadOnlyList<SeasonalCatalog>> GetBySeasonAsync(Season season)
        => catalogRepository.GetBySeasonAsync(season);

    private async Task<SeasonalCatalog> GetCatalogOrThrowAsync(SeasonalCatalogId id)
    {
        var catalog = await catalogRepository.GetByIdAsync(id);
        if (catalog is null)
            throw new EntityNotFoundException($"SeasonalCatalog {id.Value} not found.");
        return catalog;
    }
}
