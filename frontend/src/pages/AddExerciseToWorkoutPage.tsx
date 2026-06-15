import React, { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
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
      navigate(`/workouts/${workoutId}`);
    } catch (err: any) {
      console.error(err);
      setError(err.message || 'Kunde inte lägga till exercise');
    }
  };

  if (loading) return <div className="page"><p className="status-msg">Laddar övningar...</p></div>;
  if (error) return <div className="page"><p className="error-msg">{error}</p></div>;

  return (
    <div className="page">
      <Link to={`/workouts/${workoutId}`} className="back-link">&larr; Tillbaka</Link>
      <h2>Lägg till övning</h2>

      {exercises.length === 0 ? (
        <p className="status-msg">Inga övningar tillgängliga</p>
      ) : (
        <div className="exercise-grid">
          {exercises.map((exercise) => (
            <div className="exercise-item" key={exercise.id}>
              <span>{exercise.name}</span>
              <button className="btn btn-primary" onClick={() => handleAddExercise(exercise.id)}>
                Lägg till
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default AddExerciseToWorkoutPage;
