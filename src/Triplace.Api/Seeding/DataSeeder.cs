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

        // Bazylika Mariacka — zwiedzanie wnętrza
// Pon-Sob: 11:30-18:00, Niedziela i święta: 14:00-18:00
// Bilet normalny: 20 zł, ulgowy: 10 zł (uczniowie 7-18 lat, studenci 19-26 lat, seniorzy 65+, KDR)
var kosciolId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
    "Bazylika Mariacka — Zwiedzanie Bazyliki",
    AttractionCategory.Church,
    new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
    VisitDuration.Short,
    IsOutdoor: false,
    IsFree: false,
    new HashSet<AttractionAmenity> { AttractionAmenity.GuideAvailable, AttractionAmenity.BlindFriendly },
    [
        new MetadataEntry("adres",          "Plac Mariacki 5, 31-042 Kraków"),
        new MetadataEntry("godziny",        "Pon-Sob 11:30-18:00; Niedziela i święta 14:00-18:00"),
        new MetadataEntry("bilet_normalny", "20 zł"),
        new MetadataEntry("bilet_ulgowy",   "10 zł (uczniowie 7-18 lat, studenci 19-26 lat, seniorzy 65+, KDR)")
    ]));

// Bazylika Mariacka — Hejnalica (wieża północna)
// Czynna od 10 kwietnia; Pt-Nd 10:10-17:30, wejścia co 30 min
// Max 15 osób na wejście; dzieci < 7 lat: wstęp wzbroniony
// Bilet normalny: 20 zł, ulgowy: 15 zł; zakup tylko w dniu wizyty, brak rezerwacji online
var hejnalicaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
    "Bazylika Mariacka — Hejnalica (Wieża Północna)",
    AttractionCategory.Church,
    new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
    VisitDuration.Short,
    IsOutdoor: false,
    IsFree: false,
    new HashSet<AttractionAmenity> { AttractionAmenity.GuideAvailable },
    [
        new MetadataEntry("adres",            "Plac Mariacki 5 (wejście od ul. Floriańskiej), 31-042 Kraków"),
        new MetadataEntry("godziny",          "Pt-Nd 10:10-17:30, wejścia co 30 min"),
        new MetadataEntry("sezon",            "od 10 kwietnia"),
        new MetadataEntry("max_osob",         "15 osób na wejście"),
        new MetadataEntry("ograniczenia",     "Dzieci poniżej 7 lat: wstęp wzbroniony"),
        new MetadataEntry("bilet_normalny",   "20 zł"),
        new MetadataEntry("bilet_ulgowy",     "15 zł"),
        new MetadataEntry("sprzedaz_biletow", "Tylko w dniu wizyty, brak rezerwacji online")
    ]));

        var auschwitzId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Muzeum Auschwitz",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Long,
            IsOutdoor: false,
            IsFree: true,
            new HashSet<AttractionAmenity> { AttractionAmenity.AudioGuide, AttractionAmenity.GuideAvailable, AttractionAmenity.WheelchairAccess },
            [new MetadataEntry("adres", "Więźniów Oświęcimia 20, 32-603 Oświęcim")]));

        // Wawel — sub-atrakcje (dane: wawel.krakow.pl/co-zwiedzac)
        var zamekId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Zamek Królewski na Wawelu",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
            VisitDuration.Long,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.AudioGuide, AttractionAmenity.GuideAvailable, AttractionAmenity.WheelchairAccess, AttractionAmenity.GiftShop },
            [new MetadataEntry("adres", "Wawel 5, 31-001 Kraków"), new MetadataEntry("bilet_normalny", "95 zł")]));

        var podziemiaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Podziemia Zamku — Wawel Zaginiony",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
            VisitDuration.Medium,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.AudioGuide, AttractionAmenity.GuideAvailable },
            [new MetadataEntry("adres", "Wawel 5, 31-001 Kraków"), new MetadataEntry("bilet_normalny", "47 zł")]));

        var skarbiecId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Skarbiec Koronny",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
            VisitDuration.Medium,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.GuideAvailable, AttractionAmenity.WheelchairAccess },
            [new MetadataEntry("adres", "Wawel 5, 31-001 Kraków"), new MetadataEntry("bilet_normalny", "47 zł")]));

        var zbrojowniaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Zbrojownia",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
            VisitDuration.Short,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.GuideAvailable },
            [new MetadataEntry("adres", "Wawel 5, 31-001 Kraków"), new MetadataEntry("bilet_normalny", "47 zł")]));

        var smoczaJamaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Smocza Jama",
            AttractionCategory.Entertainment,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Short,
            IsOutdoor: true,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.FamilyFriendly },
            [new MetadataEntry("adres", "Wawel 5, 31-001 Kraków"), new MetadataEntry("bilet_normalny", "15 zł"), new MetadataEntry("sezon", "27 kwietnia – 31 października")]));

        var ogrodyId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Ogrody Królewskie",
            AttractionCategory.Park,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Short,
            IsOutdoor: true,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.FamilyFriendly, AttractionAmenity.WheelchairAccess },
            [new MetadataEntry("adres", "Wawel 5, 31-001 Kraków"), new MetadataEntry("bilet_normalny", "11 zł"), new MetadataEntry("sezon", "24 kwietnia – 4 października")]));

        var basztaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Baszta Widokowa",
            AttractionCategory.NaturalSite,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Short,
            IsOutdoor: true,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.ParkingNearby },
            [new MetadataEntry("adres", "Wawel 5, 31-001 Kraków"), new MetadataEntry("bilet_normalny", "19 zł"), new MetadataEntry("sezon", "24 kwietnia – 31 października")]));

        // 2. Publish all
        foreach (var id in new[] { wawelId, rynekId, sukienniceId, kazimierzId, kosciolId, hejnalicaId, auschwitzId,
                     zamekId, podziemiaId, skarbiecId, zbrojowniaId, smoczaJamaId, ogrodyId, basztaId })
            await attractionService.PublishAsync(id);

        // 3. Hierarchy: Sukiennice i Kościół Mariacki są pod Rynkiem Głównym
        await attractionService.AddChildAsync(rynekId, sukienniceId);
        await attractionService.AddChildAsync(rynekId, kosciolId);
        await attractionService.AddChildAsync(kosciolId, hejnalicaId);

        // Wawel — zagnieżdżona hierarchia sub-atrakcji
        await attractionService.AddChildAsync(wawelId, zamekId);
        await attractionService.AddChildAsync(wawelId, podziemiaId);
        await attractionService.AddChildAsync(wawelId, skarbiecId);
        await attractionService.AddChildAsync(wawelId, zbrojowniaId);
        await attractionService.AddChildAsync(wawelId, smoczaJamaId);
        await attractionService.AddChildAsync(wawelId, ogrodyId);
        await attractionService.AddChildAsync(wawelId, basztaId);

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
        // Bazylika rekomenduje Sukiennice (sąsiedztwo na Rynku)
        await relationService.AddRecommendationAsync(
            new AttractionId((await attractionService.GetByIdAsync(kosciolId))!.Id.Value),
            new AttractionId((await attractionService.GetByIdAsync(sukienniceId))!.Id.Value));

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
