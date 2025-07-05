# 🎛️ Wrecept - Project Overview

**A retro-modern invoice recording desktop application inspired by DOS-era logistics systems**

## 🌟 What is Wrecept?

Wrecept is an **offline-first, single-user WPF application** designed for recording and managing invoices with a focus on speed, reliability, and keyboard-only operation. The application draws inspiration from classic DOS-era business software like Clipper and dBase IV systems, offering a nostalgic yet modern approach to invoice management.

## 🎯 Core Philosophy

- **No mouse required** - Everything can be done with Enter, Esc, ↑, ↓ keys
- **Retro aesthetics** - DOS-style interface with color-coded panels and full-screen efficiency
- **Offline-first** - No network dependency, works completely offline
- **Predictable and stable** - Designed to be foolproof with graceful error handling
- **Keyboard-driven workflow** - Optimized for users familiar with legacy DOS interfaces

## 🏗️ Architecture

The application follows a clean **layered architecture**:

```
┌─────────────────────────────────────────────┐
│                   WPF UI                    │
│            (Views & Themes)                 │
├─────────────────────────────────────────────┤
│                ViewModels                   │
│           (MVVM Pattern)                    │
├─────────────────────────────────────────────┤
│                Core Layer                   │
│        (Business Logic & Services)          │
├─────────────────────────────────────────────┤
│               Storage Layer                 │
│        (Entity Framework + SQLite)          │
└─────────────────────────────────────────────┘
```

### Project Structure

- **Wrecept.Core** - Domain models, business logic, and service interfaces
- **Wrecept.Storage** - Entity Framework Core with SQLite persistence
- **Wrecept.Wpf** - WPF user interface and ViewModels
- **Wrecept.UiTests** - UI automation tests
- **docs/** - Comprehensive documentation
- **tools/** - Helper scripts and utilities

## 🎹 Key Features

### Currently Implemented
- ✅ **Retro-style UI** with DOS-inspired color schemes
- ✅ **Keyboard-only navigation** (Enter/Esc/↑/↓)
- ✅ **Structured main menu** with StageView layout
- ✅ **Invoice editor** with header and line item management
- ✅ **Master data management** (Products, Suppliers, Tax Rates, etc.)
- ✅ **SQLite database** with Entity Framework Core
- ✅ **Progress reporting** with dual progress bars
- ✅ **Startup orchestration** with database seeding
- ✅ **Comprehensive error handling** and logging

### Planned Features
- 🔄 **Audio-visual feedback** for user actions
- 🔄 **Backup & recovery** after power outages
- 🔄 **Advanced reporting** and data export
- 🔄 **Print functionality** for invoices

## 💼 Business Logic

### Invoice Management
- Create and edit invoices with multiple line items
- Support for both **gross** and **net** pricing modes
- Automatic calculation of totals with VAT breakdown
- **Archival system** - invoices can be archived to prevent modification
- Support for **returns** (negative quantities)

### Master Data
- **Suppliers** - Vendor information and contact details
- **Products** - Product catalog with pricing and tax information
- **Tax Rates** - Configurable VAT rates with date validity
- **Payment Methods** - Various payment options (cash, card, transfer)
- **Product Groups** - Categorization for better organization
- **Units** - Measurement units (kg, pieces, etc.)

### Data Flow
```
User Input → ViewModel → Core Services → Storage Layer → SQLite Database
```

## 🛠️ Technology Stack

- **Framework**: .NET 8
- **UI**: WPF (Windows Presentation Foundation)
- **Database**: SQLite with Entity Framework Core
- **MVVM**: CommunityToolkit.Mvvm
- **Testing**: UI automation tests
- **Architecture**: Clean Architecture with DI container

## 📁 File System Layout

```
%AppData%\Wrecept\
├── app.db               # SQLite database
├── backup\              # Scheduled backups
├── settings.json        # User preferences
├── wrecept.json         # User/company information
├── logs\                # Runtime error logs
└── version.txt          # Application version tracking
```

## 🔧 Development Approach

### Agent-Based Development
The project uses an **agent-based development model** with specialized roles:

- **root_agent** - Project coordination and architecture decisions
- **core_agent** - Business logic and domain models
- **storage_agent** - Database and persistence layer
- **code_agent** - ViewModels and application logic
- **ui_agent** - User interface and themes
- **logic_agent** - User interaction and keyboard handling
- **test_agent** - Testing and quality assurance
- **docs_agent** - Documentation and specifications

### Progress Tracking
- **Detailed progress logs** in `docs/progress/` with timestamped entries
- **Milestone tracking** with clear deliverables
- **Feature planning** with agent coordination
- **Task management** through structured documentation

## 🎨 User Interface

### Design Principles
- **Full-screen efficiency** - No wasted screen space
- **Color-coded panels** for different functional areas
- **Keyboard shortcuts** displayed in footers
- **Consistent navigation** patterns throughout
- **Retro terminal aesthetics** with modern usability

### Themes
- **RetroTheme.xaml** - Primary DOS-inspired color scheme
- **Extensible theming** system for future customization
- **Consistent styling** across all controls and views

## 🔍 Current Status

### What's Working
- Basic application structure and navigation
- Invoice editor with line item management
- Master data CRUD operations
- Database migrations and seeding
- Progress reporting during startup
- Keyboard navigation framework

### What's In Progress
- Advanced keyboard workflows
- Data validation and error handling
- Audio-visual feedback systems
- Comprehensive testing coverage

### What's Planned
- Printing and export functionality
- Advanced reporting features
- Data import from legacy systems
- Enhanced backup and recovery

## 📚 Documentation

The project includes extensive documentation:

- **Architecture guides** - System design and patterns
- **Business logic** - Domain rules and calculations
- **Development specs** - Technical requirements
- **User manuals** - End-user documentation (EN/HU)
- **Agent coordination** - Development workflow
- **Error handling** - Fault tolerance strategies
- **Testing strategy** - Quality assurance approach

## 🚀 Getting Started

### Prerequisites
- Windows 10+ (x64)
- .NET 8 Runtime
- No network connection required

### Build & Run
```bash
# Clone the repository
git clone https://github.com/luckydizzier/wrecept.git

# Build the solution
dotnet build

# Run the application
dotnet run --project Wrecept.Wpf
```

### Development
```bash
# Run tests
dotnet test

# Generate database migrations
dotnet ef migrations add <MigrationName> --project Wrecept.Storage
```

## 🤝 Contributing

The project follows a structured agent-based development model. See `docs/AGENTS.md` for role definitions and `docs/TASKLOG.md` for current priorities.

## 📄 License

This project is a work in progress and not yet intended for production use.

---

*"A színpad áll, a keverő bekészítve. Most jöhet a kábelezés."* - The stage is set, the mixer is ready. Now comes the wiring.

**Wrecept** - Where nostalgia meets modern reliability.