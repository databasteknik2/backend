# Backend – Utbildningsföretagets API

Detta är backend-systemet för ett utbildningsföretag, utvecklat med **ASP.NET Core Minimal API**.

Systemet hanterar:
- Kurser
- Kurstillfällen
- Lärare
- Deltagare
- Registreringar till kurser

Projektet är byggt med fokus på **skalbar arkitektur, dataintegritet och testbarhet**.

---

## 🏗 Arkitektur

Projektet följer principerna för:

- Domain-Driven Design (DDD)
- Clean Architecture

Projektet är uppdelat i följande lager:

### Domain
Innehåller:
- Entiteter
- Affärslogik
- Domänregler

Exempel:
- Course
- Enrollment
- Teacher
- Participant

---

### Application
Innehåller:
- DTOs
- Services
- Interfaces

Här ligger applikationslogiken som kommunicerar med domän- och infrastrukturlagret.

---

### Infrastructure
Ansvarar för:
- Databasåtkomst
- Entity Framework Core
- Migrations
- SQLite databasen

---

### Presentation
Innehåller:
- Minimal API endpoints
- Routing
- API konfiguration

---

### Tests
Innehåller:
- Enhetstester
- Integrationstester mot in-memory databas

---

## 🚀 Teknologier

- ASP.NET Core 10 Minimal API
- Entity Framework Core (Code First)
- SQLite
- Memory Cache
- Swagger
- xUnit Testing
- Raw SQL queries via EF Core

---

## ⚡ Funktioner

### Kursadministration
- Skapa kurser
- Uppdatera kurser
- Ta bort kurser
- Lista kurser

---

### Kurstillfällen
- Skapa kurstillfällen
- Uppdatera tider och plats
- Ta bort kurstillfällen

---

### Registrering
- Deltagare kan registrera sig på kurstillfällen
- Transaktionshantering säkerställer dataintegritet

---

### Prestandaoptimering
- Caching av kursdata via IMemoryCache
- AsNoTracking för läsoperationer

---

### Databas
Systemet använder en normaliserad relationsdatabas (3NF) med tabeller:

- Courses  
- CourseEvents  
- Teachers  
- Participants  
- Enrollments  

---

## 🛠 Installation & Körning

### 1. Klona projektet
```bash
git clone <repo-url>
2. Navigera till backend projektet
cd Backend
3. Installera beroenden
dotnet restore
4. Kör projektet
dotnet run

Backend startar normalt på:
http://localhost:5054

5. Testa API

Öppna Swagger:
http://localhost:5054/swagger


🧪 Tester

Kör tester med:
dotnet test

Tester inkluderar:
Enhetstester för domänlogik
Integrationstester mot SQLite in-memory databas

🔐 Säkerhet & Dataintegritet

Transaktioner används vid kursanmälan
Unika constraints på email för deltagare
Cascade delete för relationer

Utvecklad som inlämningsprojekt för databasteknikkurs.

```
