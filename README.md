GameCollectionApp 🎮

📋 Project Overview

GameCollectionApp is a web application for managing personal video game collections. Built with .NET 9.0 and Razor Pages, it allows users to catalog games, organize collections, rate titles, and track gaming statistics. The project implements a layered architecture with Domain-Driven Design principles.

✨ Key Features

    🎮 Game Management: Add, edit, and organize video games with details

    📊 Collections: Create personal game collections

    ⭐ Ratings & Notes: Rate games and keep notes 

    🔍 Advanced Search: Filter games by platform, genre, year, and more

    👤 User Profiles: Personal dashboard with gaming statistics

    🛡️ Authentication: Secure login with role-based permissions (Admin/User)    

📁 Project Structure

    GameCollectionApp/
├── GameCollection.Domain/          # Domain Layer (Entities, Value Objects, Domain Services)
├── GameCollection.Application/     # Application Layer (Use Cases, DTOs, Application Services)
├── GameCollection.Infrastructure/  # Infrastructure Layer (Data Access, External Services)
├── GameCollection.API/             # Presentation Layer (Razor Pages, Controllers)
└── GameCollection.Tests/           # Test Projects (Unit & Integration Tests)

🚀 Quick Setup
Prerequisites

    .NET 9.0 SDK

    SQL Server (or SQL Server Express)

    Visual Studio 2022 or VS Code

Installation Steps

    Clone the repository
    bash

    git clone https://github.com/Morfeas98/GameCollectionApp.git
    cd GameCollectionApp

    Configure database connection

        Open GameCollection.API/appsettings.json

        Update the connection string if needed (default uses LocalDB)

    Setup the database
    bash

    cd GameCollection.Web
    dotnet ef database update

    *This creates the database with sample data (50+ games, platforms, users)*

    Run the application
    bash

    dotnet run

    Or open in Visual Studio and press F5

    Access the application

        Open browser to: https://localhost:5001

        Default login: admin@gamecollection.com / Admin123!

🏗️ Project Structure

The application follows a layered architecture:

    Domain Layer: Core business entities and logic

    Application Layer: Use cases and business services

    Infrastructure Layer: Data access and external services

    Web Layer: Razor Pages UI and presentation logic

📧 Contact

GitHub: Morfeas98
Repository: GameCollectionApp
