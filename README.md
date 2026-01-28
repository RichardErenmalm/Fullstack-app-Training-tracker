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

Backend:
Gå till backend-mappen i terminalen och kör:
dotnet restore
dotnet build
dotnet ef database update --project Infrastructure--startup-project API
dotnet run

Frontend:
Gå till frontend-mappen i terminalen och kör:
npm install
npm start

Miljövariabler:
VITE_API_URL=http://localhost:5000

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
