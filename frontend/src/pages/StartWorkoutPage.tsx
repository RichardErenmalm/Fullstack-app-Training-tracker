import { useParams, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import axios from 'axios';
import { WorkoutExercise } from '../types/WorkoutExercise';
import { Exercise } from '../types/Exercise';

type ExerciseSetInput = {
  setNumber: number;
  reps: number;
  weight: number;
};

type ExerciseWithSets = {
  exerciseId: number;
  exerciseName: string;
  sets: ExerciseSetInput[];
};

const StartWorkoutPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [exercisesWithSets, setExercisesWithSets] = useState<ExerciseWithSets[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchExercisesForWorkout = async () => {
      try {
        if (!id) return;

        const weRes = await fetch(
          `https://localhost:7026/api/WorkoutExercises/workout/${id}`
        );
        if (!weRes.ok) throw new Error('Kunde inte hämta workout exercises');

        const weResult = await weRes.json();
        const workoutExercises: WorkoutExercise[] = weResult.data;

        const exercises: ExerciseWithSets[] = await Promise.all(
          workoutExercises.map(async (we) => {
            const exRes = await fetch(
              `https://localhost:7026/api/Exercises/${we.exerciseId}`
            );
            if (!exRes.ok)
              throw new Error(`Kunde inte hämta exercise ${we.exerciseId}`);
            const exercise: Exercise = await exRes.json();

            const sets: ExerciseSetInput[] = [];
            const totalSets = we.sets || 3;
            for (let i = 1; i <= totalSets; i++) {
              sets.push({ setNumber: i, reps: 0, weight: 0 });
            }

            return {
              exerciseId: we.exerciseId,
              exerciseName: exercise.name,
              sets,
            };
          })
        );

        setExercisesWithSets(exercises);
      } catch (err: any) {
        console.error(err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchExercisesForWorkout();
  }, [id]);

  const updateSet = (
    exerciseId: number,
    setNumber: number,
    field: keyof ExerciseSetInput,
    value: number
  ) => {
    setExercisesWithSets((prev) =>
      prev.map((ex) =>
        ex.exerciseId === exerciseId
          ? {
              ...ex,
              sets: ex.sets.map((s) =>
                s.setNumber === setNumber ? { ...s, [field]: value } : s
              ),
            }
          : ex
      )
    );
  };

  const saveWorkout = async () => {
    try {
      for (const ex of exercisesWithSets) {
        for (const s of ex.sets) {
          await axios.post('https://localhost:7026/api/ExerciseHistory', {
            exerciseId: ex.exerciseId,
            reps: s.reps,
            weightKg: s.weight,
            setNumber: s.setNumber,
            userId: 1, // TODO: byt till inloggad user
          });
        }
      }
      navigate(`/workouts/${id}`);
    } catch (err) {
      console.error(err);
      alert('Kunde inte spara workout');
    }
  };

  if (loading) return <p>Laddar workout...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <h2>Start Workout</h2>

      {exercisesWithSets.map((ex) => (
        <div key={ex.exerciseId} style={{ marginBottom: '1rem' }}>
          <h3>{ex.exerciseName}</h3>

          {/* Rubriker */}
          <div style={{ display: 'flex', fontWeight: 'bold', marginBottom: '0.5rem' }}>
            <div style={{ width: '60px' }}>Set</div>
            <div style={{ width: '80px' }}>Reps</div>
            <div style={{ width: '100px' }}>Weight (kg)</div>
          </div>

          {/* Rader per set */}
          {ex.sets.map((s) => (
            <div
              key={s.setNumber}
              style={{ display: 'flex', marginBottom: '0.5rem', alignItems: 'center' }}
            >
              <div style={{ width: '60px' }}>#{s.setNumber}</div>
              <input
                type="number"
                placeholder="Reps"
                value={s.reps}
                onChange={(e) =>
                  updateSet(ex.exerciseId, s.setNumber, 'reps', Number(e.target.value))
                }
                style={{ width: '80px', marginRight: '10px' }}
              />
              <input
                type="number"
                placeholder="Weight"
                value={s.weight}
                onChange={(e) =>
                  updateSet(ex.exerciseId, s.setNumber, 'weight', Number(e.target.value))
                }
                style={{ width: '100px' }}
              />
            </div>
          ))}
        </div>
      ))}

      <button onClick={saveWorkout}>Done</button>
    </div>
  );
};

export default StartWorkoutPage;
