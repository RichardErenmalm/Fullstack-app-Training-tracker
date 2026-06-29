-- Training Tracker - Seed Data (PPL-split)
-- Rensa befintlig data i rätt ordning (FK-constraints)
DELETE FROM ExerciseHistories;
DELETE FROM WorkoutHistories;
DELETE FROM WorkoutExercises;
DELETE FROM Workouts;
DELETE FROM Exercises;
DELETE FROM Users;

-- Återställ identity-räknare
DBCC CHECKIDENT ('Users', RESEED, 0);
DBCC CHECKIDENT ('Exercises', RESEED, 0);
DBCC CHECKIDENT ('Workouts', RESEED, 0);
DBCC CHECKIDENT ('WorkoutExercises', RESEED, 0);
DBCC CHECKIDENT ('WorkoutHistories', RESEED, 0);
DBCC CHECKIDENT ('ExerciseHistories', RESEED, 0);

-- Users
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (Id, Username, Password) VALUES (1, N'demo', N'password123');
SET IDENTITY_INSERT Users OFF;

-- Exercises
SET IDENTITY_INSERT Exercises ON;
INSERT INTO Exercises (Id, Name) VALUES
    (1, N'Bänkpress'),
    (2, N'Knäböj'),
    (3, N'Marklyft'),
    (4, N'Axelpress'),
    (5, N'Latsdrag'),
    (6, N'Hantelcurl');
SET IDENTITY_INSERT Exercises OFF;

-- Workouts (PPL-split)
SET IDENTITY_INSERT Workouts ON;
INSERT INTO Workouts (Id, Name, UserId) VALUES
    (1, N'Push', 1),
    (2, N'Pull', 1),
    (3, N'Legs', 1);
SET IDENTITY_INSERT Workouts OFF;

-- WorkoutExercises (övningar kopplade till pass med antal sets)
SET IDENTITY_INSERT WorkoutExercises ON;
INSERT INTO WorkoutExercises (Id, WorkoutId, ExerciseId, Sets) VALUES
    (1, 1, 1, 4),
    (2, 1, 4, 3),
    (3, 2, 3, 4),
    (4, 2, 5, 4),
    (5, 2, 6, 3),
    (6, 3, 2, 4),
    (7, 3, 3, 3);
SET IDENTITY_INSERT WorkoutExercises OFF;

-- WorkoutHistories (genomförda träningssessioner)
SET IDENTITY_INSERT WorkoutHistories ON;
INSERT INTO WorkoutHistories (Id, WorkoutId, UserId, PerformedAt) VALUES
    (1, 1, 1, '2026-06-08 10:00:00'),
    (2, 2, 1, '2026-06-15 11:00:00'),
    (3, 3, 1, '2026-06-22 10:30:00');
SET IDENTITY_INSERT WorkoutHistories OFF;

-- ExerciseHistories (loggade sets per session)
SET IDENTITY_INSERT ExerciseHistories ON;
INSERT INTO ExerciseHistories (Id, Name, WeightKg, Reps, SetNumber, ExerciseId, UserId, WorkoutExerciseId, WorkoutHistoryId) VALUES
    -- Push-session
    (1,  N'Bänkpress', 80, 8, 1, 1, 1, 1, 1),
    (2,  N'Bänkpress', 80, 8, 2, 1, 1, 1, 1),
    (3,  N'Bänkpress', 80, 7, 3, 1, 1, 1, 1),
    (4,  N'Bänkpress', 80, 6, 4, 1, 1, 1, 1),
    (5,  N'Axelpress', 50, 10, 1, 4, 1, 2, 1),
    (6,  N'Axelpress', 50,  9, 2, 4, 1, 2, 1),
    (7,  N'Axelpress', 50,  8, 3, 4, 1, 2, 1),
    -- Pull-session
    (8,  N'Marklyft', 120, 5, 1, 3, 1, 3, 2),
    (9,  N'Marklyft', 120, 5, 2, 3, 1, 3, 2),
    (10, N'Marklyft', 120, 4, 3, 3, 1, 3, 2),
    (11, N'Marklyft', 120, 4, 4, 3, 1, 3, 2),
    (12, N'Latsdrag', 70, 10, 1, 5, 1, 4, 2),
    (13, N'Latsdrag', 70, 10, 2, 5, 1, 4, 2),
    (14, N'Latsdrag', 70,  9, 3, 5, 1, 4, 2),
    (15, N'Latsdrag', 70,  8, 4, 5, 1, 4, 2),
    (16, N'Hantelcurl', 16, 12, 1, 6, 1, 5, 2),
    (17, N'Hantelcurl', 16, 11, 2, 6, 1, 5, 2),
    (18, N'Hantelcurl', 16, 10, 3, 6, 1, 5, 2),
    -- Legs-session
    (19, N'Knäböj', 100, 6, 1, 2, 1, 6, 3),
    (20, N'Knäböj', 100, 6, 2, 2, 1, 6, 3),
    (21, N'Knäböj', 100, 5, 3, 2, 1, 6, 3),
    (22, N'Knäböj', 100, 5, 4, 2, 1, 6, 3),
    (23, N'Marklyft', 125, 5, 1, 3, 1, 7, 3),
    (24, N'Marklyft', 125, 5, 2, 3, 1, 7, 3),
    (25, N'Marklyft', 125, 4, 3, 3, 1, 7, 3);
SET IDENTITY_INSERT ExerciseHistories OFF;

PRINT 'Seed klar!';
