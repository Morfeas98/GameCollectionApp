# 🎮 GameCollectionApp

##📋  Project Overview

GameCollectionApp is a web application for managing personal video game collections. Built with .NET 9.0 and Razor Pages, it allows users to catalog games, organize collections, rate titles, and track gaming statistics. The project implements a layered architecture with Domain-Driven Design principles.

## 🏗️ Αρχιτεκτονική & Δομή
Το project χωρίζεται σε τρία βασικά επίπεδα (Projects):

* **GameCollection.API**: Το επίπεδο παρουσίασης (Razor Pages & Controllers). Περιέχει όλο το UI και τη λογική των endpoints.
* **GameCollection.Infrastructure**: Το επίπεδο πρόσβασης στα δεδομένα. Περιέχει το `AppDbContext`, τα Migrations και την υλοποίηση των Repositories.
* **GameCollection.Core** (ή Domain): Περιέχει τις βασικές οντότητες (Models), τα Interfaces και τα DTOs.



## 🚀 Τεχνολογικό Stack
* **Framework**: .NET 8.0 / 9.0 (ASP.NET Core)
* **Database**: SQL Server (Entity Framework Core)
* **Frontend**: Bootstrap 5, Bootstrap Icons, JavaScript (Fetch API)
* **Authentication**: ASP.NET Core Identity

## 🛠️ Οδηγίες Εγκατάστασης

### 1. Clone the repository
```bash
    git clone https://github.com/Morfeas98/GameCollectionApp.git
    cd GameCollectionApp
```

### 2. Configure database connection

    * Open GameCollection.Web/appsettings.json
    * Update the connection string if needed (default uses LocalDB).

### 3. Εκτέλεση της Εφαρμογής
```Bash
dotnet run --project GameCollection.API
```
✨ Βασικές Λειτουργίες

    📂 Collections: Οργάνωση παιχνιδιών σε διαφορετικές λίστες (π.χ. Favorites, Backlog).

    ⭐ Rating & Notes: Δυνατότητα προσθήκης προσωπικής βαθμολογίας και αναλυτικών σημειώσεων.

    🎮 Progress Tracking: Σήμανση παιχνιδιών ως Completed ή Currently Playing.

    🔍 Game Discovery: Αναζήτηση και προσθήκη νέων τίτλων στη συλλογή.

🔑 Διαπιστευτήρια (Credentials)

    Για λόγους δοκιμής, μπορείτε να χρησιμοποιήσετε τα παρακάτω:
        
        Ρόλος	Username	Password
        
        Admin	Admin	    Admin123!
        
        User	testuser	User123!