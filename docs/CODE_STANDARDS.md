---
title: "Code Standards"
purpose: "Naming conventions and static analysis rules"
author: "docs_agent"
date: "2025-06-27"
---

# 📐 Code Standards and Static Analysis

A kód minden rétegében egységes konvenciókat követünk, hogy a projekt átlátható és hosszú távon fenntartható legyen.

## Naming

* **Namespace**: `Wrecept.<Layer>.<Module>`
* **Osztályok**: PascalCase
* **Mezők**: `_camelCase`
* **Fájlok**: az osztály nevét viselik

## Warning kezelés

* A fordító figyelmeztetéseit nem nyomjuk el. A kódnak tisztán kell fordulnia `-warnaserror` beállítás mellett is.
* A `nullable` referencia típusok használata kötelező a null hibák elkerülésére.

## Kódgenerálás

* A CommunityToolkit.Mvvm `ObservableProperty` és `RelayCommand` attribútumait használjuk a ViewModelben.
* Az automatikusan generált fájlokat a `Generated` mappában tartjuk, hogy elkülönüljenek a kézzel írt kódtól.

---
