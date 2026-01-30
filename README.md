GameCollectionApp 🎮

https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet
https://img.shields.io/badge/C%2523-239120?logo=csharp
https://img.shields.io/badge/Razor%2520Pages-5C2D91?logo=dotnet
https://img.shields.io/badge/SQL%2520Server-CC2927?logo=microsoftsqlserver
https://img.shields.io/badge/License-MIT-green

📋 Project Overview
GameCollectionApp is a comprehensive video game collection management system developed as a university assignment. The application implements Domain-Driven Design (DDD) principles with a layered architecture, providing a complete solution for gamers to organize, track, and rate their video game collections.

📁 Project Structure
GameCollectionApp/
├── GameCollection.Domain/          # Domain Layer (Entities, Value Objects, Domain Services)
├── GameCollection.Application/     # Application Layer (Use Cases, DTOs, Application Services)
├── GameCollection.Infrastructure/  # Infrastructure Layer (Data Access, External Services)
├── GameCollection.API/             # Presentation Layer (Razor Pages, Controllers)
└── GameCollection.Tests/           # Test Projects (Unit & Integration Tests)

🚀 Getting Started
Prerequisites

    .NET 9.0 SDK

    SQL Server 2022 or SQL Server Express

    Visual Studio 2022 or VS Code

    Git

Installation & Setup

    Clone the repository
    bash

    git clone https://github.com/Morfeas98/GameCollectionApp.git
    cd GameCollectionApp

    Configure the database connection

        Open GameCollection.Web/appsettings.json

        Update the connection string to match your SQL Server instance:
    json

    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GameCollectionDb;Trusted_Connection=True;MultipleActiveResultSets=true"
      }
    }

    Apply database migrations and seed data
    bash

    cd GameCollection.Web
    dotnet ef database update

    This will create the database schema and populate it with initial seed data including:

        50+ video games across different platforms

        17 gaming platforms

        22 game genres

        27 franchises

        11 users with different roles

    Build and run the application
    bash

    dotnet build
    dotnet run

    Or open the solution in Visual Studio and press F5.

    Access the application

        Main application: https://localhost:5001

        Swagger API documentation: https://localhost:5001/swagger

        Database: Can be viewed via SQL Server Management Studio

Default User Accounts

    Administrator: admin@gamecollection.com / Admin123!

    Moderator: moderator@gamecollection.com / Mod123!

    Regular User: user@gamecollection.com / User123!
