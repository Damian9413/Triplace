using Triplace.Application.Commands;
using Triplace.Application.Services;
using Triplace.Domain.Builders;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Api.Seeding;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var attractionService = services.GetRequiredService<AttractionService>();
        var groupService = services.GetRequiredService<AttractionGroupService>();
        var addonTypeService = services.GetRequiredService<AttractionAddonTypeService>();
        var catalogService = services.GetRequiredService<SeasonalCatalogService>();
        var relationService = services.GetRequiredService<RelationService>();
        var routeService = services.GetRequiredService<RouteService>();

        // 1. Addon types
        var openingHoursId = await addonTypeService.CreateAsync(new CreateAddonTypeCommand(
            "Godziny otwarcia",
            [
                new FieldDefinitionCommand("from", FieldValueType.String, new UnconstrainedConstraint()),
                new FieldDefinitionCommand("to", FieldValueType.String, new UnconstrainedConstraint()),
                new FieldDefinitionCommand("day", FieldValueType.String, new AllowedValuesConstraint(
                    ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"]))
            ]));

        await addonTypeService.CreateAsync(new CreateAddonTypeCommand(
            "Cena biletu",
            [
                new FieldDefinitionCommand("amount", FieldValueType.Int, new UnconstrainedConstraint()),
                new FieldDefinitionCommand("currency", FieldValueType.String, new AllowedValuesConstraint(["PLN", "EUR"]))
            ]));

        var openingHoursAddon = await addonTypeService.GetByIdOrThrowAsync(openingHoursId);

        // 2. Create attractions (drafts)
        var attractionDefs = new[]
        {
            ("Wawel", "Historyczny zamek królewski na wzgórzu wawelskim"),
            ("Rynek Główny", "Największy średniowieczny rynek w Europie"),
            ("Sukiennice", "Gotycka hala targowa na Rynku Głównym"),
            ("Kazimierz", "Historyczna dzielnica żydowska"),
            ("Kościół Mariacki", "Gotycka bazylika z ołtarzem Wita Stwosza"),
            ("Muzeum Auschwitz", "Miejsce pamięci byłego obozu koncentracyjnego")
        };

        var attractionIds = new Dictionary<string, AttractionId>();
        foreach (var (name, desc) in attractionDefs)
        {
            var id = await attractionService.CreateDraftAsync(new CreateAttractionCommand(name,
                [new MetadataEntry("opis", desc)]));
            attractionIds[name] = id;
        }

        // 3. Publish all
        foreach (var id in attractionIds.Values)
            await attractionService.PublishAsync(id);

        var allAttractions = await attractionService.GetAllAsync();
        var attractionMap = allAttractions.ToDictionary(a => a.Name);

        // 4. Groups: Stare Miasto
        var staremiastoGroupId = await groupService.CreateAsync(
            new CreateAttractionGroupCommand("Kraków — Stare Miasto"));

        await groupService.AddEntryAsync(new AddGroupEntryCommand(
            staremiastoGroupId, attractionMap["Wawel"].Id.Value, "attraction",
            [new AddAddonCommand(openingHoursAddon.Id.Value,
                new Dictionary<string, object> { { "from", "09:00" }, { "to", "17:00" }, { "day", "Mon" } })]));

        await groupService.AddEntryAsync(new AddGroupEntryCommand(
            staremiastoGroupId, attractionMap["Sukiennice"].Id.Value, "attraction",
            [new AddAddonCommand(openingHoursAddon.Id.Value,
                new Dictionary<string, object> { { "from", "10:00" }, { "to", "18:00" }, { "day", "Mon" } })]));

        await groupService.AddEntryAsync(new AddGroupEntryCommand(
            staremiastoGroupId, attractionMap["Rynek Główny"].Id.Value, "attraction"));

        await groupService.AddEntryAsync(new AddGroupEntryCommand(
            staremiastoGroupId, attractionMap["Kościół Mariacki"].Id.Value, "attraction"));

        // Kazimierz group
        var kazimierzGroupId = await groupService.CreateAsync(
            new CreateAttractionGroupCommand("Kraków — Kazimierz"));

        await groupService.AddEntryAsync(new AddGroupEntryCommand(
            kazimierzGroupId, attractionMap["Kazimierz"].Id.Value, "attraction"));

        // 5. Top-level Kraków group (nested groups)
        var krakowGroupId = await groupService.CreateAsync(
            new CreateAttractionGroupCommand("Kraków"));

        await groupService.AddEntryAsync(new AddGroupEntryCommand(
            krakowGroupId, staremiastoGroupId.Value, "group"));

        await groupService.AddEntryAsync(new AddGroupEntryCommand(
            krakowGroupId, kazimierzGroupId.Value, "group"));

        // 6. Seasonal catalog
        var catalogId = await catalogService.CreateAsync(new CreateSeasonalCatalogCommand(
            "Kraków Lato 2025",
            Season.Summer,
            new DateOnly(2025, 6, 1),
            new DateOnly(2025, 8, 31),
            "Małopolska",
            "Letni katalog atrakcji Krakowa",
            500));

        // 7. Publish all attractions to catalog
        foreach (var id in attractionIds.Values)
            await catalogService.PublishToCatalogAsync(catalogId, id);

        // Load catalog entries
        var catalog = (await catalogService.GetByIdAsync(catalogId))!;
        var entryMap = catalog.Entries.ToDictionary(e => e.SnapshotName, e => e.Id);

        // 8. Exclusion: Muzeum Auschwitz ↔ Kazimierz
        await relationService.AddExclusionAsync(
            attractionMap["Muzeum Auschwitz"].Id,
            attractionMap["Kazimierz"].Id);

        // 9. Recommendation: Wawel ↔ Rynek Główny
        await relationService.AddRecommendationAsync(
            attractionMap["Wawel"].Id,
            attractionMap["Rynek Główny"].Id);

        // 10. Route: Kraków w 1 dzień
        var routeItems = new List<RouteItemCommand>();
        if (entryMap.TryGetValue("Wawel", out var wawelEntry))
            routeItems.Add(new RouteItemCommand(wawelEntry, Priority.Must));
        if (entryMap.TryGetValue("Rynek Główny", out var rynekEntry))
            routeItems.Add(new RouteItemCommand(rynekEntry, Priority.Must));
        if (entryMap.TryGetValue("Sukiennice", out var sukieEntry))
            routeItems.Add(new RouteItemCommand(sukieEntry, Priority.Must));
        if (entryMap.TryGetValue("Kazimierz", out var kazEntry))
            routeItems.Add(new RouteItemCommand(kazEntry, Priority.Optional));
        if (entryMap.TryGetValue("Kościół Mariacki", out var koscEntry))
            routeItems.Add(new RouteItemCommand(koscEntry, Priority.Optional));

        await routeService.CreateAsync(new CreateRouteCommand(
            "Kraków w 1 dzień",
            "Najważniejsze atrakcje w jeden dzień",
            Season.Summer,
            krakowGroupId,
            routeItems));
    }
}
