import { useParams, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { getWorkoutById } from '../api/workoutApi';
import { Workout } from '../types/Workout';
import { WorkoutExercise } from '../types/WorkoutExercise';
import { Exercise } from '../types/Exercise';

interface WorkoutExerciseWithName extends WorkoutExercise {
  exercise?: Exercise;
}

const WorkoutDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [workout, setWorkout] = useState<Workout | null>(null);
  const [workoutExercises, setWorkoutExercises] = useState<WorkoutExerciseWithName[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Fetcha workout exercises + exercise namn
  const fetchWorkoutExercisesWithNames = async () => {
    if (!id) return;

    try {
      // 1️⃣ Hämta alla workoutExercises
      const res = await fetch(`https://localhost:7026/api/WorkoutExercises/workout/${id}`);
      if (!res.ok) throw new Error('Kunde inte hämta exercises');
      const result = await res.json();
      const exercisesData: WorkoutExercise[] = result.data;

      // 2️⃣ Fetcha Exercise för varje exerciseId
      const exercisesWithNames: WorkoutExerciseWithName[] = await Promise.all(
        exercisesData.map(async (we) => {
          const exRes = await fetch(`https://localhost:7026/api/Exercises/${we.exerciseId}`);
          if (!exRes.ok) throw new Error(`Kunde inte hämta exercise med id ${we.exerciseId}`);
          const exResult: Exercise = await exRes.json();
          return { ...we, exercise: exResult }; // ✅ använda exResult direkt
        })
      );

      // 3️⃣ Sätt i state så frontend kan visa exercises med namn
      setWorkoutExercises(exercisesWithNames);
    } catch (err: any) {
      console.error(err);
      setError(err.message);
    }
  };

  useEffect(() => {
    const fetchWorkoutAndExercises = async () => {
      try {
        if (!id) {
          setError('Ogiltigt workout-id');
          return;
        }

        // Hämta workout
        const workoutData = await getWorkoutById(Number(id));
        setWorkout(workoutData);

        // Hämta exercises
        await fetchWorkoutExercisesWithNames();
      } catch (err: any) {
        console.error(err);
        setError(err.message || `Kunde inte hämta workout med id ${id}`);
      } finally {
        setLoading(false);
      }
    };

    fetchWorkoutAndExercises();
  }, [id]);

  if (loading) return <p>Laddar workout...</p>;
  if (error) return <p>{error}</p>;
  if (!workout) return <p>Workout hittades inte</p>;

  return (
    <div>
      <h2>{workout.name}</h2>
      <p>User ID: {workout.userId}</p>

      {/* 🔹 Enkel knapp som navigerar till Add Exercise-sidan */}
      <button onClick={() => navigate(`/workouts/${workout.id}/add-exercise`)}>
        Lägg till exercise
      </button>

      <hr />

      <h3>Exercises</h3>
      {workoutExercises.length === 0 ? (
        <p>Inga exercises tillagda än.</p>
      ) : (
        <ul>
          {workoutExercises.map((we) => (
            <li key={we.id}>
              <strong>{we.exercise?.name}</strong> <br />
              Sets: {we.sets} | Reps: {we.reps ?? ''} | Weight: {we.weight ?? 0} kg
            </li>
          ))}
        </ul>
      )}


      <button onClick={() => navigate(`/workouts/${workout.id}/start`)}>
        Start workout
      </button>

    </div>
  );
};

export default WorkoutDetailPage;
