# Triplace

REST API do zarządzania turystycznymi atrakcjami i planowania tras zwiedzania. Zbudowane w oparciu o Domain-Driven Design z warstwową architekturą.

## Stos technologiczny

- **.NET 10** / **ASP.NET Core**
- **C#** — architektura warstwowa (DDD)
- Dane przechowywane **in-memory** (brak bazy danych)
- Dokumentacja API: **Scalar** (`/scalar/v1`)

## Struktura projektu

```
src/
├── Triplace.Domain         # Encje, value objects, buildery, specyfikacje, wyjątki
├── Triplace.Application    # Interfejsy repozytoriów, serwisy, komendy
├── Triplace.Infrastructure # Repozytoria in-memory (ConcurrentDictionary, Singleton)
└── Triplace.Api            # Kontrolery, DTOs, middleware, seeder danych
```

Warstwy mają ścisłe granice: `Domain` nie ma żadnych zewnętrznych zależności, każda kolejna warstwa referencuje tylko bezpośrednio niższą.

## Uruchomienie

```bash
dotnet run --project src/Triplace.Api
```

API dostępne pod `http://localhost:5122`. Dane są seedowane automatycznie przy starcie.

## Endpointy

| Zasób | URL |
|---|---|
| Atrakcje | `GET/POST /api/attractions` |
| Katalogi sezonowe | `GET/POST /api/seasonal-catalogs` |
| Relacje | `GET/POST /api/relations` |
| Trasy | `GET/POST /api/routes` |
| Dokumentacja (Scalar) | `GET /scalar/v1` *(tylko dev)* |
| OpenAPI spec | `GET /openapi/v1.json` |

## Model domenowy

### Atrakcja

Podstawowa jednostka systemu. Tworzona jako `Draft`, następnie `Published` lub `Archived`.

Każda atrakcja posiada:
- kategorię (`Museum`, `Church`, `Park`, `NaturalSite`, `Entertainment`, `Gallery`, `Restaurant`)
- pory roku, w których jest polecana
- czas zwiedzania (`Short` < 1h / `Medium` 1–3h / `Long` > 3h)
- flagi: `IsOutdoor`, `IsFree`
- udogodnienia (AudioGuide, WheelchairAccess, FamilyFriendly itd.)
- dowolne metadane (pary klucz–wartość, np. adres, cena biletu)

Atrakcje tworzą **hierarchię drzewiastą** — atrakcja może mieć atrakcje-dzieci:

```
Wawel
├── Zamek Królewski na Wawelu
├── Skarbiec Koronny
├── Zbrojownia
├── Podziemia Zamku
├── Smocza Jama
├── Ogrody Królewskie
└── Baszta Widokowa

Rynek Główny
├── Sukiennice
└── Kościół Mariacki
```

### Katalog sezonowy

Kolekcja atrakcji promowanych w danym sezonie (np. *„Kraków Lato 2025"*). Przyjmuje tylko atrakcje ze statusem `Published`. Snapshot nazwy atrakcji jest zapisywany w momencie dodania do katalogu.

### Relacje

Relacje między dwoma atrakcjami:
- **Recommendation** — atrakcje dobrze do siebie pasują (np. Wawel i Rynek Główny)
- **Exclusion** — atrakcje nie powinny być łączone w jednej trasie (np. Auschwitz i Kazimierz)

### Trasa

Uporządkowana lista atrakcji z priorytetami (`Must` / `Optional`). Obsługuje zmianę kolejności elementów z automatycznym przenumerowaniem.

## Wzorce projektowe

- **Builder** — jedyna droga tworzenia encji domenowych (`AttractionBuilder`, `RouteBuilder`, `SeasonalCatalogBuilder`)
- **Specification** — filtrowanie atrakcji (`InCategorySpec`, `ForSeasonSpec`, `IsFreeSpec`, `HasAmenitySpec` itd.) z kompozycją `And` / `Or` / `Not`
- **Composite** — hierarchia atrakcji (rodzic zawiera dzieci, `Flatten()` zwraca całe poddrzewo)
- **Singleton registry** — `AttractionRelationRegistry` jako agregat-korzeń dla relacji

## Dane seedowane przy starcie

- 6 atrakcji krakowskich (Wawel, Rynek Główny, Sukiennice, Kazimierz, Kościół Mariacki, Muzeum Auschwitz)
- 7 sub-atrakcji Wawelu (Zamek Królewski, Skarbiec Koronny, Zbrojownia, Podziemia, Smocza Jama, Ogrody, Baszta Widokowa)
- Hierarchia: Rynek Główny → {Sukiennice, Kościół Mariacki}, Wawel → {7 sub-atrakcji}
- 1 katalog sezonowy: *„Kraków Lato 2025"* (lato 2025)
- 2 relacje: Exclusion (Auschwitz ↔ Kazimierz), Recommendation (Wawel ↔ Rynek Główny)
- 1 trasa: *„Kraków w 1 dzień"* (Wawel Must, Rynek Must, Kazimierz Optional)
