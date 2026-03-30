using Triplace.Application.Commands;
using Triplace.Application.Services;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Api.Seeding;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var attractionService = services.GetRequiredService<AttractionService>();
        var catalogService = services.GetRequiredService<SeasonalCatalogService>();
        var relationService = services.GetRequiredService<RelationService>();
        var routeService = services.GetRequiredService<RouteService>();

        // 1. Create attractions
        var wawelId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Wawel",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Long,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.AudioGuide, AttractionAmenity.GuideAvailable, AttractionAmenity.WheelchairAccess },
            [new MetadataEntry("adres", "Wawel 5, 31-001 Kraków")]));

        var rynekId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Rynek Główny",
            AttractionCategory.NaturalSite,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
            VisitDuration.Medium,
            IsOutdoor: true,
            IsFree: true,
            new HashSet<AttractionAmenity> { AttractionAmenity.FamilyFriendly, AttractionAmenity.ParkingNearby },
            [new MetadataEntry("adres", "Rynek Główny, 31-042 Kraków")]));

        var sukienniceId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Sukiennice",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
            VisitDuration.Medium,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.GiftShop, AttractionAmenity.WheelchairAccess },
            [new MetadataEntry("adres", "Rynek Główny 1-3, 31-042 Kraków")]));

        var kazimierzId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Kazimierz",
            AttractionCategory.NaturalSite,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Long,
            IsOutdoor: true,
            IsFree: true,
            new HashSet<AttractionAmenity> { AttractionAmenity.FamilyFriendly, AttractionAmenity.Cafe },
            [new MetadataEntry("adres", "Kazimierz, Kraków")]));

        var kosciolId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Kościół Mariacki",
            AttractionCategory.Church,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
            VisitDuration.Short,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.GuideAvailable, AttractionAmenity.BlindFriendly },
            [new MetadataEntry("adres", "Plac Mariacki 5, 31-042 Kraków")]));

        var auschwitzId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Muzeum Auschwitz",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Long,
            IsOutdoor: false,
            IsFree: true,
            new HashSet<AttractionAmenity> { AttractionAmenity.AudioGuide, AttractionAmenity.GuideAvailable, AttractionAmenity.WheelchairAccess },
            [new MetadataEntry("adres", "Więźniów Oświęcimia 20, 32-603 Oświęcim")]));

        // 2. Publish all
        foreach (var id in new[] { wawelId, rynekId, sukienniceId, kazimierzId, kosciolId, auschwitzId })
            await attractionService.PublishAsync(id);

        // 3. Hierarchy: Sukiennice i Kościół Mariacki są pod Rynkiem Głównym
        await attractionService.AddChildAsync(rynekId, sukienniceId);
        await attractionService.AddChildAsync(rynekId, kosciolId);

        // 4. Seasonal catalog
        var catalogId = await catalogService.CreateAsync(new CreateSeasonalCatalogCommand(
            "Kraków Lato 2025",
            Season.Summer,
            new DateOnly(2025, 6, 1),
            new DateOnly(2025, 8, 31),
            "Małopolska",
            "Letni katalog atrakcji Krakowa",
            500));

        foreach (var id in new[] { wawelId, rynekId, kazimierzId, auschwitzId })
            await catalogService.AddAttractionAsync(catalogId, id);

        // 5. Relations
        await relationService.AddExclusionAsync(
            new AttractionId((await attractionService.GetByIdAsync(auschwitzId))!.Id.Value),
            new AttractionId((await attractionService.GetByIdAsync(kazimierzId))!.Id.Value));

        await relationService.AddRecommendationAsync(
            new AttractionId((await attractionService.GetByIdAsync(wawelId))!.Id.Value),
            new AttractionId((await attractionService.GetByIdAsync(rynekId))!.Id.Value));

        // 6. Route
        var routeItems = new List<RouteItemCommand>
        {
            new(new AttractionId(wawelId.Value), Priority.Must),
            new(new AttractionId(rynekId.Value), Priority.Must),
            new(new AttractionId(kazimierzId.Value), Priority.Optional)
        };

        await routeService.CreateAsync(new CreateRouteCommand(
            "Kraków w 1 dzień",
            "Najważniejsze atrakcje w jeden dzień",
            Season.Summer,
            null,
            routeItems));
    }
}
