using Triplace.Application.Commands;
using Triplace.Application.Repositories;
using Triplace.Application.Results;
using Triplace.Domain.Builders;
using Triplace.Domain.Enums;
using Triplace.Domain.Exceptions;
using Triplace.Domain.Ids;

namespace Triplace.Application.Services;

public class RouteService(
    IRouteRepository routeRepository,
    IAttractionRelationRegistryRepository registryRepository)
{
    public async Task<RouteId> CreateAsync(CreateRouteCommand command)
    {
        var builder = RouteBuilder.Create(command.Name)
            .WithDescription(command.Description)
            .ForSeason(command.Season);

        if (command.ScopeGroupId.HasValue)
            builder.ScopedTo(command.ScopeGroupId.Value);

        foreach (var item in command.Items)
            builder.WithItem(item.AttractionId, item.Priority);

        var route = builder.Build();
        await routeRepository.SaveAsync(route);
        return route.Id;
    }

    public async Task AddItemAsync(RouteId routeId, AttractionId attractionId, Priority priority)
    {
        var route = await GetRouteOrThrowAsync(routeId);
        route.AddItem(attractionId, priority);
        await routeRepository.SaveAsync(route);
    }

    public async Task RemoveItemAsync(RouteId routeId, RouteItemId routeItemId)
    {
        var route = await GetRouteOrThrowAsync(routeId);
        route.RemoveItem(routeItemId);
        await routeRepository.SaveAsync(route);
    }

    public async Task ReorderItemAsync(RouteId routeId, RouteItemId routeItemId, int newSortOrder)
    {
        var route = await GetRouteOrThrowAsync(routeId);
        route.Reorder(routeItemId, newSortOrder);
        await routeRepository.SaveAsync(route);
    }

    public Task<Domain.Entities.Route?> GetByIdAsync(RouteId id) => routeRepository.GetByIdAsync(id);

    public Task<IReadOnlyList<Domain.Entities.Route>> GetAllAsync() => routeRepository.GetAllAsync();

    public async Task<IReadOnlyList<ExclusionConflict>> CheckExclusionsAsync(RouteId routeId)
    {
        var route = await GetRouteOrThrowAsync(routeId);
        var registry = await registryRepository.GetRegistryAsync();

        var conflicts = new List<ExclusionConflict>();
        var items = route.Items.ToList();

        for (int i = 0; i < items.Count; i++)
            for (int j = i + 1; j < items.Count; j++)
                if (registry.AreExclusive(items[i].AttractionId, items[j].AttractionId))
                    conflicts.Add(new ExclusionConflict(items[i].AttractionId, items[j].AttractionId));

        return conflicts.AsReadOnly();
    }

    private async Task<Domain.Entities.Route> GetRouteOrThrowAsync(RouteId id)
    {
        var route = await routeRepository.GetByIdAsync(id);
        if (route is null)
            throw new EntityNotFoundException($"Route {id.Value} not found.");
        return route;
    }
}
