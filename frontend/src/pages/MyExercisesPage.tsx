import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getWorkoutExercisesByWorkoutId } from '../api/workoutExerciseApi';
import { getWorkouts } from '../api/workoutApi';
import { getExerciseById } from '../api/exerciseApi';

type ExerciseInfo = {
  exerciseId: number;
  name: string;
};

const MyExercisesPage: React.FC = () => {
  const [exercises, setExercises] = useState<ExerciseInfo[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchExercises = async () => {
      try {
        const workouts = await getWorkouts();

        const allWorkoutExercises = (
          await Promise.all(
            workouts.map((w) => getWorkoutExercisesByWorkoutId(w.id))
          )
        ).flat();

        const uniqueMap = new Map<number, ExerciseInfo>();

        await Promise.all(
          allWorkoutExercises.map(async (we) => {
            if (!uniqueMap.has(we.exerciseId)) {
              const exercise = await getExerciseById(we.exerciseId);
              uniqueMap.set(we.exerciseId, {
                exerciseId: we.exerciseId,
                name: exercise.name,
              });
            }
          })
        );

        setExercises(Array.from(uniqueMap.values()));
      } catch (err: any) {
        console.error(err);
        setError(err.message || 'Kunde inte hämta övningar');
      } finally {
        setLoading(false);
      }
    };

    fetchExercises();
  }, []);

  if (loading) return <div className="page"><p className="status-msg">Laddar övningar...</p></div>;
  if (error) return <div className="page"><p className="error-msg">{error}</p></div>;

  return (
    <div className="page">
      <h2>Mina Övningar</h2>

      {exercises.length === 0 ? (
        <p className="status-msg">Du har inga övningar tillagda i någon workout ännu.</p>
      ) : (
        exercises.map((ex) => (
          <div
            className="card"
            key={ex.exerciseId}
            onClick={() => navigate(`/exercise-history/${ex.exerciseId}`, { state: { from: '/exercises' } })}
            style={{ cursor: 'pointer' }}
          >
            <div className="card-title">{ex.name}</div>
            <div className="card-subtitle">Se historik</div>
          </div>
        ))
      )}
    </div>
  );
};

export default MyExercisesPage;
