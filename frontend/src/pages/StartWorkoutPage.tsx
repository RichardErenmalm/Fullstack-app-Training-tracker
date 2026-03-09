import { useParams, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { getWorkoutExercisesByWorkoutId, saveExerciseHistory } from '../api/workoutExerciseApi';
import { getExerciseById } from '../api/exerciseApi';

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

        const workoutExercises = await getWorkoutExercisesByWorkoutId(Number(id));

        const exercises: ExerciseWithSets[] = await Promise.all(
          workoutExercises.map(async (we) => {
            const exercise = await getExerciseById(we.exerciseId);

            const totalSets = we.sets || 3;
            const sets: ExerciseSetInput[] = Array.from({ length: totalSets }, (_, i) => ({
              setNumber: i + 1,
              reps: 0,
              weight: 0,
            }));

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
          await saveExerciseHistory({
            exerciseId: ex.exerciseId,
            reps: s.reps,
            weightKg: s.weight,
            setNumber: s.setNumber,
            userId: 1, // TODO: replace with authenticated user
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

          <div style={{ display: 'flex', fontWeight: 'bold', marginBottom: '0.5rem' }}>
            <div style={{ width: '60px' }}>Set</div>
            <div style={{ width: '80px' }}>Reps</div>
            <div style={{ width: '100px' }}>Weight (kg)</div>
          </div>

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
