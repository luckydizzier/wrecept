---
title: "Inline Entity Creation"
purpose: "Rövid technikai leírás az inline entitás létrehozásáról"
author: "docs_agent"
date: "2025-06-30"
---

# Inline Entity Creation

Az InvoiceEditor nézetben lehetőség nyílik a hiányzó törzsadatok azonnali felvételére. Ha a felhasználó olyan terméket, szállítót, ÁFA-kulcsot vagy mértékegységet ír be, amely nem található az adatbázisban, a program egy kibomló űrlapot jelenít meg az aktuális sor alatt.

```text
+-----------------------+
| Termék neve nincs még |
| [Név: ________]       |
| [OK] [Mégse]          |
+-----------------------+
```

A kitöltött űrlap mentése azonnal létrehozza az entitást és a szerkesztett sorban is frissíti a hivatkozást. Minden más vezérlő ideiglenesen letiltásra kerül, hogy a fókusz az új bejegyzésen maradjon.

## Megvalósítás

* Minden entitáshoz külön `*CreatorViewModel` és XAML nézet tartozik.
* Az `InvoiceEditorViewModel.InlineCreator` property tartalmazza az aktuális űrlapot.
* A háttérszín `#F5F5DC`, ezzel emeljük ki az inline űrlapot.
* Az entitások validálását a megfelelő szolgáltatások (`IProductService`, stb.) végzik.

## Jövőbeli bővítések

* Hibakezelési üzenetek megjelenítése az űrlap alatt.
* Billentyűparancsok testreszabása.
* További mezők felvétele a részletes adatbeviteli igények szerint.
