import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getWorkouts } from '../api/workoutApi';
import { Workout } from '../types/Workout';
import CreateWorkoutForm from '../components/CreateWorkoutForm';

const WorkoutListPage: React.FC = () => {
  const [workouts, setWorkouts] = useState<Workout[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchWorkouts = async () => {
      try {
        const data = await getWorkouts(); // Workout[]
        setWorkouts(data);
      } catch (err: any) {
        console.error(err);
        setError(err.message || 'Kunde inte hämta workouts');
      } finally {
        setLoading(false);
      }
    };

    fetchWorkouts();
  }, []);

  const handleWorkoutCreated = (newWorkout: Workout) => {
    setWorkouts([...workouts, newWorkout]);
  };

  if (loading) return <p>Laddar workouts...</p>;
  if (error) return <p>{error}</p>;
  if (workouts.length === 0) return <p>Inga workouts hittades</p>;

  return (
    <div>
      <h2>Workouts</h2>
      <CreateWorkoutForm onWorkoutCreated={handleWorkoutCreated} />
      <ul>
        {workouts.map((w) => (
          <li key={w.id}>
            <Link to={`/workouts/${w.id}`}>
              <strong>{w.name}</strong>
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default WorkoutListPage;
