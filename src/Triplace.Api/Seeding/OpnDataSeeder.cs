using Triplace.Application.Commands;
using Triplace.Application.Services;
using Triplace.Domain.Enums;
using Triplace.Domain.Ids;
using Triplace.Domain.ValueObjects;

namespace Triplace.Api.Seeding;

public static class OpnDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var attractionService = services.GetRequiredService<AttractionService>();
        var catalogService = services.GetRequiredService<SeasonalCatalogService>();
        var relationService = services.GetRequiredService<RelationService>();
        var routeService = services.GetRequiredService<RouteService>();

        // 1. Atrakcje — źródło: Kompendium Atrakcji i Szlaków OPN V5

        var opnId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Ojcowski Park Narodowy",
            AttractionCategory.Park,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Long,
            IsOutdoor: true,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.FamilyFriendly, AttractionAmenity.ParkingNearby },
            [new MetadataEntry("region", "Małopolska"), new MetadataEntry("adres", "32-047 Ojców")]));

        var zamekKazimierzId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Zamek Kazimierzowski w Ojcowie",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Medium,
            IsOutdoor: true,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.GuideAvailable, AttractionAmenity.ParkingNearby },
            [
                new MetadataEntry("adres", "Ojców 3, 32-047 Ojców"),
                new MetadataEntry("bilet_normalny", "22 zł"),
                new MetadataEntry("bilet_ulgowy", "15 zł"),
                new MetadataEntry("platnosc", "wyłącznie gotówka")
            ]));

        var jaskiniaLokietkaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Jaskinia Łokietka",
            AttractionCategory.NaturalSite,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Short,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.GuideAvailable, AttractionAmenity.ParkingNearby },
            [
                new MetadataEntry("adres", "Czajowice, 32-047 Ojców"),
                new MetadataEntry("temperatura_wewnątrz", "7-8°C"),
                new MetadataEntry("parking", "Czajowice, 300 m od wejścia"),
                new MetadataEntry("szlak", "niebieski — Wąwóz Ciasne Skałki")
            ]));

        var jaskiniaCiemnaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Jaskinia Ciemna",
            AttractionCategory.NaturalSite,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Short,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.GuideAvailable },
            [
                new MetadataEntry("adres", "Ojców, nad Bramą Krakowską"),
                new MetadataEntry("historia", "Obozowisko neandertalczyków, ~50 000 lat"),
                new MetadataEntry("uwaga", "Brak oświetlenia elektrycznego — zalecana latarka"),
                new MetadataEntry("szlak", "zielony — strome podejście")
            ]));

        var bramaKrakowskaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Brama Krakowska",
            AttractionCategory.NaturalSite,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
            VisitDuration.Short,
            IsOutdoor: true,
            IsFree: true,
            new HashSet<AttractionAmenity> { AttractionAmenity.FamilyFriendly, AttractionAmenity.WheelchairAccess },
            [
                new MetadataEntry("adres", "Dolina Prądnika, Ojców"),
                new MetadataEntry("opis", "Skalna brama 15 m, Skała Jonaszówka (punkt widokowy), Źródło Miłości"),
                new MetadataEntry("gastronomia", "Pstrąg Ojcowski w pobliskich stawach hodowlanych")
            ]));

        var zamekPieskowaSkalaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Zamek w Pieskowej Skale",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Long,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.AudioGuide, AttractionAmenity.GuideAvailable, AttractionAmenity.GiftShop, AttractionAmenity.Cafe },
            [
                new MetadataEntry("adres", "Sułoszowa, 32-045 Pieskowa Skała"),
                new MetadataEntry("styl", "Renesans — arkadowy dziedziniec"),
                new MetadataEntry("uwaga", "Północna brama Parku")
            ]));

        var maczugaHerkulesaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Maczuga Herkulesa",
            AttractionCategory.NaturalSite,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn, Season.Winter },
            VisitDuration.Short,
            IsOutdoor: true,
            IsFree: true,
            new HashSet<AttractionAmenity> { AttractionAmenity.FamilyFriendly },
            [
                new MetadataEntry("adres", "u stóp Zamku w Pieskowej Skale"),
                new MetadataEntry("wysokość", "25 m"),
                new MetadataEntry("szlak", "czarny")
            ]));

        var kaplicaNaWodzieId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Kaplica \"Na Wodzie\"",
            AttractionCategory.Church,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Short,
            IsOutdoor: false,
            IsFree: true,
            new HashSet<AttractionAmenity> { AttractionAmenity.FamilyFriendly },
            [
                new MetadataEntry("adres", "Ojców, Dolina Prądnika"),
                new MetadataEntry("opis", "Unikalna konstrukcja na palach, widoczna ze szlaku czerwonego")
            ]));

        var grodziskoId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Grodzisko",
            AttractionCategory.NaturalSite,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Short,
            IsOutdoor: true,
            IsFree: true,
            new HashSet<AttractionAmenity> { AttractionAmenity.FamilyFriendly },
            [
                new MetadataEntry("adres", "Grodzisko, OPN"),
                new MetadataEntry("opis", "Kompleks Salomei z rzeźbą słonia — z dala od głównego potoku turystów")
            ]));

        var mlynBoroniaId = await attractionService.CreateDraftAsync(new CreateAttractionCommand(
            "Młyn Boronia",
            AttractionCategory.Museum,
            new HashSet<Season> { Season.Spring, Season.Summer, Season.Autumn },
            VisitDuration.Short,
            IsOutdoor: false,
            IsFree: false,
            new HashSet<AttractionAmenity> { AttractionAmenity.GuideAvailable },
            [
                new MetadataEntry("adres", "Dolina Prądnika, OPN"),
                new MetadataEntry("opis", "Najlepiej zachowany zabytek techniki w Parku"),
                new MetadataEntry("rezerwacja", "Wnętrza dostępne wyłącznie po telefonicznym umówieniu")
            ]));

        // 2. Publikacja
        foreach (var id in new[]
        {
            opnId, zamekKazimierzId, jaskiniaLokietkaId, jaskiniaCiemnaId, bramaKrakowskaId,
            zamekPieskowaSkalaId, maczugaHerkulesaId, kaplicaNaWodzieId, grodziskoId, mlynBoroniaId
        })
            await attractionService.PublishAsync(id);

        // 3. Hierarchia
        await attractionService.AddChildAsync(opnId, zamekKazimierzId);
        await attractionService.AddChildAsync(opnId, jaskiniaLokietkaId);
        await attractionService.AddChildAsync(opnId, jaskiniaCiemnaId);
        await attractionService.AddChildAsync(opnId, bramaKrakowskaId);
        await attractionService.AddChildAsync(opnId, zamekPieskowaSkalaId);
        await attractionService.AddChildAsync(opnId, kaplicaNaWodzieId);
        await attractionService.AddChildAsync(opnId, grodziskoId);
        await attractionService.AddChildAsync(opnId, mlynBoroniaId);

        // Maczuga Herkulesa jako sub-atrakcja Zamku w Pieskowej Skale
        await attractionService.AddChildAsync(zamekPieskowaSkalaId, maczugaHerkulesaId);

        // 4. Katalog sezonowy
        var catalogId = await catalogService.CreateAsync(new CreateSeasonalCatalogCommand(
            "OPN Lato 2025",
            Season.Summer,
            new DateOnly(2025, 5, 1),
            new DateOnly(2025, 9, 30),
            "Małopolska",
            "Letni katalog atrakcji Ojcowskiego Parku Narodowego",
            500));

        foreach (var id in new[] { zamekKazimierzId, jaskiniaLokietkaId, bramaKrakowskaId, zamekPieskowaSkalaId })
            await catalogService.AddAttractionAsync(catalogId, id);

        // 5. Relacje
        await relationService.AddRecommendationAsync(
            new AttractionId((await attractionService.GetByIdAsync(jaskiniaLokietkaId))!.Id.Value),
            new AttractionId((await attractionService.GetByIdAsync(jaskiniaCiemnaId))!.Id.Value));

        await relationService.AddRecommendationAsync(
            new AttractionId((await attractionService.GetByIdAsync(zamekKazimierzId))!.Id.Value),
            new AttractionId((await attractionService.GetByIdAsync(bramaKrakowskaId))!.Id.Value));

        // 6. Trasy
        var klasycznyDzienItems = new List<RouteItemCommand>
        {
            new(new AttractionId(zamekKazimierzId.Value), Priority.Must),
            new(new AttractionId(jaskiniaLokietkaId.Value), Priority.Must),
            new(new AttractionId(bramaKrakowskaId.Value), Priority.Must),
            new(new AttractionId(zamekPieskowaSkalaId.Value), Priority.Optional)
        };

        await routeService.CreateAsync(new CreateRouteCommand(
            "OPN - Klasyczny Dzień",
            "Najważniejsze atrakcje Ojcowskiego Parku Narodowego w jeden dzień",
            Season.Summer,
            null,
            klasycznyDzienItems));

        var szlakOrlichGniazd = new List<RouteItemCommand>
        {
            new(new AttractionId(zamekKazimierzId.Value), Priority.Must),
            new(new AttractionId(bramaKrakowskaId.Value), Priority.Must),
            new(new AttractionId(maczugaHerkulesaId.Value), Priority.Optional)
        };

        await routeService.CreateAsync(new CreateRouteCommand(
            "Szlak Orlich Gniazd (Czerwony)",
            "Główna oś Parku, 13,6 km dnem doliny — polecany dla wózków inwalidzkich i dziecięcych",
            Season.Summer,
            null,
            szlakOrlichGniazd));
    }
}
