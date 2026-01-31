# 🎮 GameCollection App

[cite_start]A professional video game collection management system built with **ASP.NET Core Razor Pages**[cite: 1].

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap)
![License](https://img.shields.io/badge/License-MIT-green.svg)

## 📖 Table of Contents
* [Overview](#-overview)
* [Features](#-features)
* [Quick Start](#-quick-start)
* [Architecture](#-architecture)
* [Project Structure](#-project-structure)
* [Installation Guide](#-installation-guide)
* [Pages & Functionality](#-pages--functionality)
* [Authentication & Authorization](#-authentication--authorization)
* [API Endpoints](#-api-endpoints)
* [Testing](#-testing)
* [Screenshots](#-screenshots)
* [Contributing](#-contributing)
* [License](#-license)

---

## 🎯 Overview
[cite_start]Το **GameCollection** είναι μια ολοκληρωμένη εφαρμογή ιστού για τη διαχείριση της συλλογής βιντεοπαιχνιδιών σας[cite: 2]. [cite_start]Σας επιτρέπει να παρακολουθείτε παιχνίδια που κατέχετε, θέλετε να παίξετε ή έχετε ολοκληρώσει, να τα οργανώνετε σε προσαρμοσμένες συλλογές και να ανακαλύπτετε νέους τίτλους μέσω προτάσεων[cite: 2].

### Γιατί GameCollection;
* [cite_start]**✅ Ολοκληρωμένη Διαχείριση** - Προσθήκη, επεξεργασία και οργάνωση των παιχνιδιών σας[cite: 3].
* [cite_start]**✅ Προσωποποιημένη Εμπειρία** - Προσθήκη αξιολογήσεων, σημειώσεων και κατάστασης ολοκλήρωσης[cite: 3].
* [cite_start]**✅ Έξυπνες Προτάσεις** - Λήψη προτάσεων βάσει της συλλογής σας[cite: 3].
* [cite_start]**✅ Σύγχρονο & Responsive** - Λειτουργεί άψογα σε desktop, tablet και κινητά[cite: 3].

---

## ✨ Features

### 🎮 Game Management
* [cite_start]**Πλήρεις λειτουργίες CRUD** - Δημιουργία, ανάγνωση, ενημέρωση και διαγραφή παιχνιδιών[cite: 4].
* [cite_start]**Πλούσιες Λεπτομέρειες** - Πληροφορίες για τίτλο, περιγραφή, έτος κυκλοφορίας, προγραμματιστή, εκδότη και genres[cite: 4].
* [cite_start]**Metacritic Integration** - Εμφάνιση βαθμολογιών και συνδέσμων για κριτικές στο Metacritic[cite: 4].
* [cite_start]**Υποστήριξη Εικόνων** - Covers παιχνιδιών με πλήρη απόκριση στη σχεδίαση[cite: 4].
* [cite_start]**Προηγμένη Αναζήτηση** - Αναζήτηση βάσει τίτλου, είδους, πλατφόρμας ή έτους[cite: 4].

### 📚 Collection Management
* [cite_start]**Προσαρμοσμένες Συλλογές** - Δημιουργήστε απεριόριστες λίστες (Favorites, Backlog, κλπ.)[cite: 5].
* [cite_start]**Σημειώσεις & Rating** - Προσθέστε προσωπική βαθμολογία (1-10) και αναλυτικές σημειώσεις[cite: 5].
* [cite_start]**Status Tracking** - Σήμανση παιχνιδιών ως "Completed" ή "Currently Playing"[cite: 5].
* [cite_start]**Μαζικές Ενέργειες** - Δυνατότητα αφαίρεσης παιχνιδιού από όλες τις συλλογές ταυτόχρονα[cite: 5].

---

## 🚀 Quick Start

### Prerequisites
* [cite_start].NET 8.0 SDK [cite: 6]
* [cite_start]SQL Server 2019+ ή SQL Server Express [cite: 6]
* [cite_start]Visual Studio 2022 ή VS Code [cite: 6]

### One-Command Setup
```bash
# Κλωνοποιήστε το repository
git clone [https://github.com/Morfeas98/GameCollectionApp.git](https://github.com/Morfeas98/GameCollectionApp.git)
cd GameCollectionApp

# Επαναφορά εξαρτήσεων
dotnet restore

# Ενημέρωση βάσης δεδομένων
dotnet ef database update

# Εκτέλεση εφαρμογής
dotnet run
```
[cite_start]Επισκεφθείτε το `https://localhost:5001` στον browser σας! [cite: 6]

---

## 🏗️ Architecture
[cite_start]Το project ακολουθεί τις αρχές του **Clean Architecture** για πλήρη διαχωρισμό των ευθυνών[cite: 8].



### Technology Stack
* [cite_start]**Backend**: ASP.NET Core 8.0, Entity Framework Core, SQL Server[cite: 7].
* [cite_start]**Frontend**: Razor Pages, Bootstrap 5, JavaScript, jQuery[cite: 7].
* [cite_start]**Patterns**: Repository Pattern, Dependency Injection, Service Layer, DTO Pattern[cite: 7, 8].

---

## 📁 Project Structure
```text
GameCollectionApp/
[cite_start]├── GameCollection.Web/                 # Presentation Layer (Pages & Controllers) [cite: 9, 11]
[cite_start]├── GameCollection.Application/         # Application Layer (Services & DTOs) [cite: 12]
[cite_start]├── GameCollection.Domain/              # Domain Layer (Entities & Interfaces) [cite: 13]
[cite_start]└── GameCollection.Infrastructure/      # Infrastructure Layer (Data & Repositories) [cite: 14]
```

---

## 🔧 Installation Guide

### Step 1: Database Setup
[cite_start]Ενημερώστε το connection string στο `appsettings.json` του Web project[cite: 16]:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GameCollectionDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Step 2: Apply Migrations
```bash
dotnet ef database update --project GameCollection.Infrastructure --startup-project GameCollection.Web
```

---

## 🔐 Authentication & Authorization
* [cite_start]**Security**: Password Hashing, Session Management (Cookies) και CSRF Protection[cite: 19].
* **Roles**:
    * [cite_start]**Guest**: Προβολή παιχνιδιών μόνο[cite: 20].
    * [cite_start]**User**: Διαχείριση συλλογών, βαθμολογιών και σημειώσεων[cite: 20].
    * [cite_start]**Admin**: Πλήρης διαχείριση παιχνιδιών, πλατφορμών και genres[cite: 20].

---

## 🛠️ API Endpoints
[cite_start]Η εφαρμογή παρέχει ένα πλήρες RESTful API[cite: 21]:

| Endpoint | Method | Description |
| :--- | :--- | :--- |
| `/api/games` | GET | [cite_start]Λήψη όλων των παιχνιδιών [cite: 21] |
| `/api/games/{id}` | GET | [cite_start]Λήψη συγκεκριμένου παιχνιδιού [cite: 22] |
| `/api/collections` | POST | [cite_start]Δημιουργία νέας συλλογής [cite: 23] |
| `/api/collections/{id}/games` | POST | [cite_start]Προσθήκη παιχνιδιού σε συλλογή [cite: 23] |

---

## 🧪 Testing
```bash
# Εκτέλεση όλων των tests
[cite_start]dotnet test [cite: 24]
```
[cite_start]Περιλαμβάνει Unit Tests για τα Services και Integration Tests για τα API Endpoints[cite: 24].

---

## 📸 Screenshots
* [cite_start]![Game Details](https://via.placeholder.com/800x450/0d6efd/ffffff?text=Game+Details+Page) [cite: 25]
* [cite_start]![Collections](https://via.placeholder.com/800x450/198754/ffffff?text=Collections+Page) [cite: 25]

---

## 🤝 Contributing
[cite_start]Τα contributions είναι ευπρόσδεκτα! [cite: 25]
1. [cite_start]Κάντε Fork το repository[cite: 25].
2. [cite_start]Δημιουργήστε ένα feature branch[cite: 25].
3. [cite_start]Ανοίξτε ένα Pull Request[cite: 25].

---

## 📄 License
[cite_start]Αυτό το project αδειοδοτείται υπό την άδεια **MIT**[cite: 27].

---
**Built with ❤️ by [Morfeas98]**
**Last Updated: January 2026**
