import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getExercises } from '../api/exerciseApi';
import { addExerciseToWorkout } from '../api/workoutExerciseApi';
import { Exercise } from '../types/Exercise';

const AddExerciseToWorkoutPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const workoutId = Number(id);
  const navigate = useNavigate();

  const [exercises, setExercises] = useState<Exercise[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchExercises = async () => {
      try {
        const data = await getExercises();
        setExercises(data);
      } catch (err: any) {
        console.error(err);
        setError(err.message || 'Kunde inte hämta exercises');
      } finally {
        setLoading(false);
      }
    };

    fetchExercises();
  }, []);

  const handleAddExercise = async (exerciseId: number) => {
    try {
      await addExerciseToWorkout(workoutId, exerciseId);
      alert('Exercise tillagd!');
      navigate(`/workouts/${workoutId}`);
    } catch (err: any) {
      console.error(err);
      setError(err.message || 'Kunde inte lägga till exercise');
    }
  };

  if (loading) return <p>Laddar exercises...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <h2>Lägg till Exercise till Workout #{workoutId}</h2>
      {exercises.length === 0 ? (
        <p>Inga exercises tillgängliga</p>
      ) : (
        <ul>
          {exercises.map((exercise) => (
            <li key={exercise.id}>
              <button onClick={() => handleAddExercise(exercise.id)}>
                {exercise.name}
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default AddExerciseToWorkoutPage;
