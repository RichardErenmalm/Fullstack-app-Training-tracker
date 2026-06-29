Training Tracker

Projektbeskrivning
Fullstack-app för att skapa, följa och uppdatera träningspass och övningar.
Backend: .NET 8, ASP.NET Core Web API
Frontend: React
Databas: EF Core (Code First)
Arkitektur: Clean Architecture + CQRS + MediatR

Arkitekturöversikt

Lagerstruktur:

Domain: Modeller (User, Workout, Exercise, WorkoutExercise, ExerciseHistory). Ansvar: domänlogik och relationer.

Application: Commands/Queries, Handlers, DTOs, Interfaces, OperationResult, AutoMapper. Ansvar: affärslogik, validering, mapping.

Infrastructure: DbContext, Repositories. Ansvar: databasåtkomst.

API: Controllers, Dependency Injection. Ansvar: tar emot HTTP-förfrågningar och skickar till handlers.

UnitTests: xUnit, Moq, FluentAssertions. Ansvar: tester av handlers med mockade repos.

Flöde: Controller → Command/Query → Handler → Repository → Response

Databasrelationer:

User → Workouts, ExerciseHistories

Workout ↔ Exercise via WorkoutExercise (many-to-many)

ExerciseHistory ↔ User/Exercise (many-to-one)

Starta projekt

Snabbstart (Windows):
Kör följande kommandon från projektroten i en PowerShell-terminal:

1. Starta backend och frontend:
.\start.ps1

2. Seeda testdata (första gången, efter att backend startat):
.\seed.ps1

Öppna sedan http://localhost:3000 i webbläsaren.

Manuell start:

Backend:
Gå till backend/Training-tracker-backend/API och kör:
dotnet restore
dotnet ef database update --project ../Infrastructure --startup-project .
dotnet run --launch-profile https

Backend körs på https://localhost:7026

Frontend:
Gå till frontend-mappen och kör:
npm install
npm start

Frontend körs på http://localhost:3000

Miljövariabler (frontend/.env):
REACT_APP_API_URL=https://localhost:7026/api

Seeda testdata

Kör seed.ps1 från projektroten efter att databasen är skapad (kräver att backend körts minst en gång så att tabellerna finns):
.\seed.ps1

Skapar följande testdata:
- 1 demoanvändare (demo / password123)
- 6 övningar (Bänkpress, Knäböj, Marklyft, Axelpress, Latsdrag, Hantelcurl)
- 3 pass (Push, Pull, Legs)
- 3 genomförda träningssessioner med 25 loggade sets och realistisk viktprogression

Endpoints

Exercise:
GET /api/Exercises
POST /api/Exercises
GET /api/Exercises/{id}
PUT /api/Exercises/{id}
DELETE /api/Exercises/{id}

ExerciseHistory:
GET /api/ExerciseHistory
POST /api/ExerciseHistory
GET /api/ExerciseHistory/{id}
PUT /api/ExerciseHistory/{id}
DELETE /api/ExerciseHistory/{id}

User:
GET /api/User
POST /api/User
GET /api/User/{id}
PUT /api/User/{id}
DELETE /api/User/{id}

Workout:
GET /api/Workouts
POST /api/Workouts
GET /api/Workouts/{id}
PUT /api/Workouts/{id}
DELETE /api/Workouts/{id}

WorkoutExercise:
GET /api/WorkoutExercises
POST /api/WorkoutExercises
GET /api/WorkoutExercises/{id}
PUT /api/WorkoutExercises/{id}
DELETE /api/WorkoutExercises/{id}
GET /api/WorkoutExercises/workout/{workoutId}

Kända buggar: Inga just nu
