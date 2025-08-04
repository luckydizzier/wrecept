# Wrecept

Wrecept is an offline-first invoicing desktop application built with C# (.NET 8) and WPF using the MVVM pattern. It focuses on efficiency and keyboard-centric operation, making it ideal for power users and point-of-sale environments.

## Features

- **Offline-first architecture** – The application works without an internet connection and synchronises data once connectivity is available.
- **Keyboard-centric UI** – All operations can be performed via keyboard shortcuts, streamlining workflows.
- **Invoice Editor** – A dynamic invoice editor allows adding items, modifying quantities, and calculating totals in real time.
- **Model–Repository–Service (MRS) architecture** – A clear separation of concerns between domain models, repositories for persistence, and services for business logic.
- **SQLite data storage** – Invoices and settings are persisted locally using a lightweight SQLite database with Entity Framework Core.
- **Theme support** – Customisable themes with a dedicated Theme Editor.
- **Logging with Serilog** – Structured logging to help diagnose issues and maintain reliability.
- **Localisation** – Support for Hungarian user interface and error messages.
- **Import/Export** – Planned support for importing and exporting database data for backups or migrations.

## Installation

> **Note:** A detailed installation guide will be added soon. For now, ensure you have .NET 8 SDK installed and run the application by building the solution.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
