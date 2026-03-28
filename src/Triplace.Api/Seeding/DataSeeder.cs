using Triplace.Application.Commands;
using Triplace.Application.Services;
using Triplace.Domain.Builders;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;
using Season = Triplace.Domain.Enums.Season;

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
            new CreateAttractionCommand(
                "Wawel",
                AttractionCategory.Museum,
                new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
                VisitDuration.Long,
                IsOutdoor: false,
                IsFree: false,
                [new MetadataEntry("opis", "Historyczny zamek królewski na wzgórzu wawelskim")]),

            new CreateAttractionCommand(
                "Rynek Główny",
                AttractionCategory.NaturalSite,
                new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
                VisitDuration.Medium,
                IsOutdoor: true,
                IsFree: true,
                [new MetadataEntry("opis", "Największy średniowieczny rynek w Europie")]),

            new CreateAttractionCommand(
                "Sukiennice",
                AttractionCategory.Museum,
                new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
                VisitDuration.Medium,
                IsOutdoor: false,
                IsFree: false,
                [new MetadataEntry("opis", "Gotycka hala targowa na Rynku Głównym")]),

            new CreateAttractionCommand(
                "Kazimierz",
                AttractionCategory.NaturalSite,
                new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
                VisitDuration.Long,
                IsOutdoor: true,
                IsFree: true,
                [new MetadataEntry("opis", "Historyczna dzielnica żydowska")]),

            new CreateAttractionCommand(
                "Kościół Mariacki",
                AttractionCategory.Church,
                new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
                VisitDuration.Short,
                IsOutdoor: false,
                IsFree: false,
                [new MetadataEntry("opis", "Gotycka bazylika z ołtarzem Wita Stwosza")]),

            new CreateAttractionCommand(
                "Muzeum Auschwitz",
                AttractionCategory.Museum,
                new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
                VisitDuration.Long,
                IsOutdoor: false,
                IsFree: true,
                [new MetadataEntry("opis", "Miejsce pamięci byłego obozu koncentracyjnego")])
        };

        var attractionIds = new Dictionary<string, AttractionId>();
        foreach (var command in attractionDefs)
        {
            var id = await attractionService.CreateDraftAsync(command);
            attractionIds[command.Name] = id;
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

        // 7. Add all attractions to catalog
        foreach (var id in attractionIds.Values)
            await catalogService.AddAttractionAsync(catalogId, id);

        // 8. Exclusion: Muzeum Auschwitz ↔ Kazimierz
        await relationService.AddExclusionAsync(
            attractionMap["Muzeum Auschwitz"].Id,
            attractionMap["Kazimierz"].Id);

        // 9. Recommendation: Wawel ↔ Rynek Główny
        await relationService.AddRecommendationAsync(
            attractionMap["Wawel"].Id,
            attractionMap["Rynek Główny"].Id);

        // 10. Route: Kraków w 1 dzień
        var routeItems = new List<RouteItemCommand>
        {
            new(attractionMap["Wawel"].Id, Priority.Must),
            new(attractionMap["Rynek Główny"].Id, Priority.Must),
            new(attractionMap["Sukiennice"].Id, Priority.Must),
            new(attractionMap["Kazimierz"].Id, Priority.Optional),
            new(attractionMap["Kościół Mariacki"].Id, Priority.Optional)
        };

        await routeService.CreateAsync(new CreateRouteCommand(
            "Kraków w 1 dzień",
            "Najważniejsze atrakcje w jeden dzień",
            Season.Summer,
            krakowGroupId,
            routeItems));
    }
}
