# Backend
# Utbildningsföretaget - Backend API

Detta är backend-systemet för ett utbildningsföretag, byggt med **ASP.NET Core 10 Minimal API**. Systemet hanterar kurser, lärare, deltagare och registreringar.

## 🏗 Arkitektur
Projektet är uppbyggt enligt principerna för **Domain-Driven Design (DDD)** och **Clean Architecture**, uppdelat i följande lager:

* **Domain:** Innehåller entiteter (t.ex. `Course`, `Enrollment`) och affärslogik.
* **Application:** Innehåller DTOs, interfaces och tjänster (t.ex. `CourseService`).
* **Infrastructure:** Hanterar databaskoppling via Entity Framework Core och SQLite.
* **Presentation (API):** Hanterar endpoints och Minimal API-konfiguration.
* **Tests:** Innehåller både enhetstester för domänen och integrationstester för databaslogiken.

## 🚀 Teknologier & Funktioner
- **Entity Framework Core (Code First):** För hantering av relationsdatabasen (SQLite).
- **Caching:** Implementerad med `IMemoryCache` i `CourseService` för optimerad prestanda.
- **Transaktionshantering:** Används vid anmälningar för att säkerställa dataintegritet.
- **Rå SQL:** Används via `FromSqlRaw` för specifika statistikfrågor.
- **Swagger:** Används för dokumentation och testning av API-endpoints.

## 🛠 Installation & Körning

1.  **Klona repot:**
    ```bash
    git clone https://github.com/databasteknik2/backend
    ```

2.  **Gå till API-mappen:**
    ```bash
    cd Backend/Backend.Presentation.API
    ```

3.  **Starta projektet:**
    ```bash
    dotnet run
    ```

4.  **Öppna Swagger:**
    När projektet körs, navigera till `http://localhost:5054/swagger/index.html` för att se och testa alla endpoints. 

## 🧪 Tester
För att köra systemets tester (Unit & Integration):
```bash
dotnet test
```

## Databasmodell
Systemet använder en normaliserad relationsdatabas (3NF) med följande huvudtabeller:

- Courses (Kurser)
- CourseEvents (Kurstillfällen)
- Teachers (Lärare)
- Participants (Deltagare)
- Enrollments (Anmälningar)
