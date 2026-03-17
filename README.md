# Triplace — Aplikacja do tworzenia tras zwiedzania

## Opis

Triplace to system wspierający planowanie zwiedzania. Na podstawie wybranej lokalizacji i preferencji użytkownika generuje propozycję trasy, którą użytkownik może dowolnie edytować i zapisać jako finalną listę atrakcji do odwiedzenia.

---

## Cel aplikacji

- Ułatwienie wyboru miejsc wartych odwiedzenia
- Skrócenie czasu planowania zwiedzania
- Tworzenie sensownych i spójnych tras
- Wspieranie użytkownika przez rekomendacje dopasowane do jego zainteresowań

---

## Działanie z perspektywy użytkownika

### 1. Wybór lokalizacji
Użytkownik wybiera miasto, które chce zwiedzać (np. Kraków, Warszawa).

### 2. Określenie preferencji
Użytkownik wskazuje obszary zainteresowań, np.:
- historia i kultura
- architektura
- spacery i parki
- gastronomia

### 3. Wygenerowanie draftu trasy
System tworzy wstępną propozycję listy atrakcji dopasowaną do:
- wybranego miasta
- preferencji użytkownika (na podstawie tagów atrakcji)

### 4. Edycja propozycji
Użytkownik może modyfikować draft:
- usuwać atrakcje, które go nie interesują
- dodawać inne atrakcje z katalogu
- zmieniać kolejność pozycji na liście
- oznaczać atrakcje jako **must-have** lub **opcjonalne**

### 5. Wsparcie systemu
Podczas edycji system może:
- rekomendować dodatkowe atrakcje powiązane z już wybranymi
- ostrzegać przed konfliktami (atrakcje, które nie powinny być łączone w jednej trasie)

### 6. Zapisanie finalnej trasy
Użytkownik zatwierdza listę — powstaje gotowa trasa zwiedzania.

---

## Mechanizm działania systemu

### Dobór atrakcji
Atrakcje są dobierane na podstawie:
- lokalizacji (miasto)
- kategorii i tagów przypisanych do atrakcji
- preferencji podanych przez użytkownika

### Rekomendacje
System sugeruje dodatkowe atrakcje na podstawie powiązań między nimi — np. *„jeśli wybierasz Wawel, warto zobaczyć też Katedrę Wawelską"*.

### Wykluczenia
System uwzględnia proste reguły wykluczeń — pewne atrakcje nie powinny pojawiać się razem w jednej trasie.

---

## Główne elementy systemu

| Element | Opis |
|---|---|
| **Atrakcja** | Pojedyncze miejsce możliwe do odwiedzenia |
| **Katalog atrakcji** | Zbiór wszystkich dostępnych atrakcji w systemie |
| **Draft trasy** | Wstępna propozycja listy atrakcji generowana przez system |
| **Trasa** | Finalna lista atrakcji zatwierdzona przez użytkownika |
| **Relacje między atrakcjami** | Powiązania rekomendacyjne i reguły wykluczeń |

---

## Założenia techniczne

- Architektura warstwowa: **API / Application / Domain / Infrastructure**
- REST API (ASP.NET Core)
- Brak autentykacji w bieżącej fazie
- Dane przechowywane in-memory (docelowo możliwa wymiana na bazę danych)
- Projekt w .NET, język C#
