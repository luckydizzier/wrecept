---
title: "Code Standards"
purpose: "Naming conventions and static analysis rules"
author: "docs_agent"
date: "2025-06-27"
---

# 📐 Code Standards and Static Analysis

A kód minden rétegében egységes konvenciókat követünk, hogy a projekt átlátható és hosszú távon fenntartható legyen.

## Naming

* **Namespace**: `InvoiceApp.<Layer>.<Module>`
* **Osztályok**: PascalCase
* **Mezők**: `_camelCase`
* **Fájlok**: az osztály nevét viselik
* **Metódusok és tulajdonságok**: PascalCase

## Warning kezelés

* A fordító figyelmeztetéseit nem nyomjuk el. A kódnak tisztán kell fordulnia `-warnaserror` beállítás mellett is.
* A `nullable` referencia típusok használata kötelező a null hibák elkerülésére.

## Null-kezelés

* Minden publikus metódus paramétereit `ArgumentNullException.ThrowIfNull()` hívással ellenőrizzük.
* A ViewModelben default értékeket adunk meg, hogy XAML-ből kötve se lehessen üres referencia.

## Kódgenerálás

* A CommunityToolkit.Mvvm `ObservableProperty` és `RelayCommand` attribútumait használjuk a ViewModelben.
* Az automatikusan generált fájlokat a `Generated` mappában tartjuk, hogy elkülönüljenek a kézzel írt kódtól.

## Verziózási szabályok

* A `main` ágba kizárólag olyan commit kerülhet, amelyhez kódfedettségi jelentés tartozik.
* A merge csak akkor engedélyezett, ha minden itt rögzített szabálynak megfelel a kód.

---
