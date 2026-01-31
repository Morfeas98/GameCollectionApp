# 🎮 GameCollection App

[cite_start]A professional video game collection management system built with **ASP.NET Core Razor Pages**. [cite: 1]

![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-9.0-512BD4?logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap)
![License](https://img.shields.io/badge/License-MIT-green.svg)

## 📖 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Quick Start](#-quick-start)
- [Project Structure](#-project-structure)
- [Installation Guide](#-installation-guide)

---

## 🎯 Overview

GameCollection is a full-featured web application for managing your video game collection. [cite: 1] [cite_start]It allows you to track games you own, want to play, or have completed, organize them into custom collections, add personal ratings and notes, and discover new games through intelligent recommendations.

### Why GameCollection?

- ✅ **Complete Collection Management** - Add, edit, organize, and track your games 
- ✅ **Personalized Experience** - Add custom ratings, notes, and completion status 
- ✅ **Smart Recommendations** - Get suggestions based on your collection 
- ✅ **Modern & Responsive** - Works on desktop, tablet, and mobile 

---

## ✨ Features

### 🎮 Game Management
- **Full CRUD Operations** - Create, Read, Update, Delete games 
- **Rich Game Details** - Title, description, release year, developer, publisher, franchises, platforms, genres 
- **Metacritic Integration** - Display scores and links to Metacritic reviews
- **Image Support** - Responsive game cover images
- **Advanced Search** - Search games by title, genre, platform, or year 

### 📚 Collection Management
- **Custom Collections** - Create unlimited collections (Favorites, Backlog, etc.) 
- **Multi-Collection Support** - Add games to multiple collections
- **Personal Notes & Ratings** - Add personal ratings (1-10) and detailed notes 
- **Status Tracking** - Mark games as "Completed" or "Currently Playing" 
- **Bulk Operations** - Remove games from all collections at once 

### 👤 User Experience
- **User Authentication** - Secure registration and login system
- **Personal Dashboard** - View your collections and statistics
- **Interactive UI** - Modern Bootstrap 5 interface with responsive design
- **Real-time Feedback** - Success/error messages with animations and confirmation dialogs

---

## 🚀 Quick Start

### Prerequisites
- .NET 9.0 SDK
- SQL Server 2019+ or SQL Server Express 
- Visual Studio 2022 or VS Code 

### One-Command Setup
```bash
# Clone the repository
git clone [https://github.com/Morfeas98/GameCollectionApp.git](https://github.com/Morfeas98/GameCollectionApp.git)
cd GameCollectionApp

# Restore dependencies
dotnet restore

# Run database migrations (Ensure connection string is set in appsettings.json)
dotnet ef database update

# Run the application
dotnet run
```
`https://localhost:5001` in your browser!

---

## 🏗️ Architecture

### Technology Stack
- **Backend**: ASP.NET Core 9.0, Entity Framework Core, SQL Server 
- **Frontend**: Razor Pages, Bootstrap 5, JavaScript, jQuery 
- **Authentication**: ASP.NET Core Identity with Cookie Authentication 
- **API**: RESTful API endpoints for AJAX operations 
- **Mapping**: AutoMapper for DTO transformations 

### Design Patterns
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Loose coupling and testability 
- **Service Layer** - Business logic encapsulation 
- **DTO Pattern** - Data transfer objects for API communication 
- **Clean Architecture** - Separation of concerns 

---

## 📁 Project Structure
```text
GameCollectionApp/
[cite_start]├── GameCollection.API/                 # Main Web Application [cite: 7]
[cite_start]│   ├── Pages/                          # Razor Pages [cite: 8]
[cite_start]│   ├── Controllers/                    # API Controllers [cite: 9]
[cite_start]│   └── Program.cs                      # Application startup [cite: 10]
[cite_start]├── GameCollection.Application/         # Application Layer (Services & DTOs) [cite: 10, 11]
[cite_start]├── GameCollection.Domain/              # Domain Layer (Entities & Repository Interfaces) [cite: 11, 12]
[cite_start]└── GameCollection.Infrastructure/      # Infrastructure Layer (DbContext & Migrations) [cite: 12, 13]
```

---

## 🔧 Installation Guide

### Step 1: Database Setup
Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GameCollectionDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Step 2: Application Configuration
```bash
# Restore NuGet packages
dotnet restore

# Apply database migrations
dotnet ef database update --project GameCollection.Infrastructure
```

---

## 🔐 Authentication & Authorization
- **Security**: BCrypt password hashing, secure cookie-based sessions, and CSRF protection 
- **User Roles**:
    - **Guest**: Browse public games and view details 
    - **User**: Manage personal collections, ratings, notes and add/edit games 
    - **Admin**: All user permissions plus delete games 

---

## 🛠️ API Endpoints
The application includes a RESTful API for AJAX operations: 

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| **GET** | `/api/games` | [cite_start]Get all games [cite: 19] |
| **GET** | `/api/games/{id}` | [cite_start]Get specific game [cite: 20] |
| **POST** | `/api/collections` | [cite_start]Create collection [cite: 21] |
| **DELETE** | `/api/collections/{id}/games/{gameId}` | [cite_start]Remove game from collection [cite: 22] |

---

## 📄 License
This project is licensed under the **MIT License**.  

---
**Built with ❤️ by [Morfeas98]**
**Last Updated: January 2024**
