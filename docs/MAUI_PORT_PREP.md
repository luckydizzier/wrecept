---
title: "MAUI Port Preparation"
purpose: "Audit notes for moving Wrecept to .NET MAUI"
author: "docs_agent"
date: "2025-07-13"
---

# MAUI Port Preparation

This document summarizes the first assessment steps toward moving the Wrecept application to .NET MAUI. The focus is on locating WPF‑specific usage and verifying how portable the Core and Storage projects are.

## WPF‑specific APIs

A repository search revealed numerous references to `System.Windows.*` in the `Wrecept.Wpf` project. Affected areas include:

- **Converters** (`Converters/*`)
- **Dialogs** (`Dialogs/DialogHelper.cs`, `Dialogs/EditEntityDialog.cs`)
- **Services** (`Services/*` such as `FocusManager`, `KeyboardManager`, `NavigationService`)
- **ViewModels** relying on `Dispatcher` or `RoutedEventArgs`
- **Views and code‑behind files** (`Views/**/*.xaml.cs`, `MainWindow.xaml.cs`)
- **MessageBoxNotificationService** which uses `System.Windows.MessageBox`

All of these must be replaced by MAUI counterparts or refactored into cross‑platform abstractions.

## Core and Storage portability

`Wrecept.Core` and `Wrecept.Storage` contain no references to `System.Windows` types. They only depend on .NET Standard APIs and Entity Framework Core, so the source can be reused in MAUI after renaming the namespaces.

Environment-specific paths are currently built with `Environment.GetFolderPath`. In MAUI this should use `FileSystem.AppDataDirectory` as noted in `ServiceCollectionExtensions.cs`.

## Planned solution restructuring

To align with the MAUI architecture, the projects will be renamed:

- **InvoiceApp.Core** – shared models and service interfaces
- **InvoiceApp.Data** – EF Core `DbContext` and repositories
- **InvoiceApp.MAUI** – MAUI UI with Views, ViewModels and startup logic

The namespaces will follow the new project names (e.g. `InvoiceApp.Core.Models`).

Initial placeholder projects have been created under InvoiceApp.Core, InvoiceApp.Data and InvoiceApp.MAUI.
