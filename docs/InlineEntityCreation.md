---
title: "Inline Entity Creation"
purpose: "Rövid technikai leírás az inline entitás létrehozásáról"
author: "docs_agent"
date: "2025-07-03"
---

# Inline Entity Creation

Az InvoiceEditor nézetben lehetőség nyílik a hiányzó törzsadatok azonnali felvételére. Ha a felhasználó olyan terméket, szállítót, ÁFA-kulcsot vagy mértékegységet ír be, amely nem található az adatbázisban, a program a beviteli mezőben felugró űrlapot jelenít meg.

Korábban a űrlap a DataGrid `RowDetailsTemplate` elemében jelent meg, ám ez rejtve maradt a sorok összecsukása miatt.  A legutóbbi frissítés óta a nézet egy lebegő `Popup` segítségével közvetlenül a keresőmező alatt jeleníti meg az űrlapot, így a felhasználó fókusza nem vész el.

```text
+-----------------------+
| Termék neve nincs még |
| [Név: ________]       |
| [OK] [Mégse]          |
+-----------------------+
```

Az űrlap megnyitásakor a kurzor automatikusan az első mezőre kerül, így a
felhasználó további billentyűk nélkül kezdheti el a gépelést.

A kitöltött űrlap mentése azonnal létrehozza az entitást és a szerkesztett sorban is frissíti a hivatkozást. Minden más vezérlő ideiglenesen letiltásra kerül, hogy a fókusz az új bejegyzésen maradjon.

## Megvalósítás

* Minden entitáshoz külön `*CreatorViewModel` és XAML nézet tartozik.
* Az `InvoiceEditorViewModel.InlineCreator` property tartalmazza az aktuális űrlapot.
* Az `InlineCreatorTarget` property az adott vezérlőre mutat, amelyhez a `Popup` igazodik.
* A háttérszín `#F5F5DC`, ezzel emeljük ki az inline űrlapot.
* Az entitások validálását a megfelelő szolgáltatások (`IProductService`, stb.) végzik.

## Jövőbeli bővítések

* Hibakezelési üzenetek megjelenítése az űrlap alatt.
* Billentyűparancsok testreszabása.
* További mezők felvétele a részletes adatbeviteli igények szerint.
