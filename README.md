# 🎮 GameCollection App

A professional video game collection management system built with **ASP.NET Core Razor Pages**.

![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-9.0-512BD4?logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap)
![License](https://img.shields.io/badge/License-MIT-green.svg)

## 📖 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Installation Guide](#-installation-guide)
- [Architecture](#%EF%B8%8F-architecture)
- [Project Structure](#-project-structure)
- [Authentication & Authorization](#-authentication--authorization)
- [API Endpoints & Future Development](#%EF%B8%8F-api-endpoints--future-development)

---

## 🎯 Overview

GameCollection is a full-featured web application for managing your video game collection. It allows you to track games you own, want to play, or have completed, organize them into custom collections, add personal ratings and notes, and discover new games through intelligent recommendations.

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

## 🔧 Installation Guide

### Prerequisites
- .NET 9.0 SDK
- SQL Server 2019+ or SQL Server Express 
- Visual Studio 2022 or VS Code 

### Installation
```bash
# Clone the repository
git clone https://github.com/Morfeas98/GameCollectionApp

cd GameCollectionApp

# Restore dependencies
dotnet restore

## Troubleshooting NuGet Sources
# If you get an error that packages (like AutoMapper) cannot be found, 
# you may need to add the official NuGet feed to your local configuration and then use dotnet restore again.
dotnet nuget add source [https://api.nuget.org/v3/index.json](https://api.nuget.org/v3/index.json) -n nuget.org

# Install/Update EF Tools globally
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

# Database Setup
cd GameCollection.Infrastructure
dotnet ef database update -s ../GameCollection.API

# Run the application
cd ../GameCollection.API
dotnet run

```

`http://localhost:5113` in your browser!

---

## 🏗️ Architecture

### Technology Stack
- **Backend**: ASP.NET Core 9.0, Entity Framework Core, SQL Server 
- **Frontend**: Razor Pages, Bootstrap 5, JavaScript, jQuery 
- **Authentication**: ASP.NET Core Identity with Cookie Authentication 
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
├── GameCollection.API/                 # Main Web Application 
│   ├── Pages/                          # Razor Pages 
│   ├── Controllers/                    # API Controllers 
│   └── Program.cs                      # Application startup 
├── GameCollection.Application/         # Application Layer (Services & DTOs) 
├── GameCollection.Domain/              # Domain Layer (Entities & Repository Interfaces)
└── GameCollection.Infrastructure/      # Infrastructure Layer (DbContext & Migrations) 
```

## 🔐 Authentication & Authorization
- **Security**: BCrypt password hashing, secure cookie-based sessions, and CSRF protection 
- **User Roles**:
    - **Guest**: Browse public games and view details 
    - **User**: Manage personal collections, ratings, notes and add/edit games 
    - **Admin**: All user permissions plus delete games 

---

## 🛠️ API Endpoints & Future Development
**Note:** The project was originally intended to have a React frontend. Due to time constraints, the current implementation uses **Razor Pages**.

The application includes a RESTful API for AJAX operations for future user: 
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| **GET** | `/api/games` | Get all games |
| **GET** | `/api/games/{id}` | Get specific game |
| **POST** | `/api/collections` | Create collection |
| **DELETE** | `/api/collections/{id}/games/{gameId}` | Remove game from collection |

