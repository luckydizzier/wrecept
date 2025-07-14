# 🎛️ AGENTS – InvoiceApp.MAUI
---
title: "MAUI Agent Guide"
purpose: "Responsibilities for UI and interaction agents"
author: "root_agent"
date: "2025-07-08"
---

## 🎨 ui_agent
**Role:** Designs and maintains MAUI XAML user interface components.
Inputs:
- ViewModel structures
- `Resources/Styles/RetroTheme.xaml`
Outputs:
- `.xaml` files under `Views/`
- DataTemplates for dynamic loading
Invariants:
- Must not call services or persistence directly
Postcondition:
- Views compile and render without errors

## 🧠 logic_agent
**Role:** Manages input handling, navigation, and modal logic.
Inputs:
- UI control tree
- ViewModel commands
Outputs:
- Event handlers and navigation state logic
Invariants:
- Persistence deferred to `core_agent`
Postcondition:
- Key events and modals behave as expected

## 🧑‍💻 code_agent
**Role:** Scaffolds ViewModels and binds UI to domain services.
Inputs:
- Domain models
- UI requirements
Outputs:
- ViewModel classes implementing `INotifyPropertyChanged`
- Command implementations
Invariants:
- Must not modify domain model logic
Postcondition:
- Project builds without compile errors

## 🔊 feedback_agent
**Role:** Provides non-intrusive user feedback.
Inputs:
- Key events and error states
Outputs:
- Sound triggers and status bar messages
Invariants:
- Must not interfere with navigation or persistence
Postcondition:
- Users receive consistent feedback cues
