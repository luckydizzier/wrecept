# Wrecept

Wrecept is an offline-first invoicing desktop application built with C# (.NET 8) and WPF using the MVVM pattern. It focuses on efficiency and keyboard-centric operation, making it ideal for power users and point-of-sale environments.

<!-- Main window screenshot omitted due to binary file restrictions -->

## Features

- **Offline-first architecture** – Works fully without internet connectivity and synchronizes data once a connection is restored.
- **Keyboard-centric UI** – Every action is driven by keyboard shortcuts for maximum speed.
- **Main menu navigation** – Quick-access categories (`Accounts`, `Stocks`, `Lists`, `Maintenance`, `Contacts`) streamline common tasks.
- **Invoice editor** – Inline item entry with real-time calculations and row-by-row validation.
- **Model–Repository–Service (MRS) architecture** – Clear separation between domain models, persistence repositories, and business services.
- **SQLite data storage** – Local persistence using a lightweight SQLite database via Entity Framework Core.
- **Theme support** – Customisable themes with a dedicated Theme Editor.
- **Logging with Serilog** – Structured logging to help diagnose issues and maintain reliability.
- **Localisation** – Hungarian user interface and error messages.
- **Import/Export (planned)** – Upcoming database backup and migration support.
## Smart Features
- **Auto-suggestions for recurring invoice items or frequently purchased products**
- **Predictive text entry using local history** 
## In-App Help & Support
- **Contextual tooltips** -
- **Quick keyboard cheat sheet** -
- **User onboarding tour** -

## Menu Structure

The main menu provides the following sections:

- **Accounts** – Manage invoices and financial records.
- **Stocks** – Record incoming delivery notes and track inventory.
- **Lists** – Access reference tables like products or tax rates.
- **Maintenance** – Application settings, theme editing, and database utilities.
- **Contacts** – Manage supplier and customer information.

## Installation

See the [installation guide](docs/INSTALL.md) for prerequisites and step-by-step setup instructions. In short, ensure the .NET 8 SDK is installed and run the application by building the solution.

## Progress Logs

Ongoing development milestones are recorded in [`docs/progress/`](docs/progress/).

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
