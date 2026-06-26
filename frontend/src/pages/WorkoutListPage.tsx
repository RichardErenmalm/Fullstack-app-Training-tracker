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
        const data = await getWorkouts();
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

  if (loading) return <div className="page"><p className="status-msg">Laddar workouts...</p></div>;
  if (error) return <div className="page"><p className="error-msg">{error}</p></div>;

  return (
    <div className="page">
      <h2>Mina Workouts</h2>
      <CreateWorkoutForm onWorkoutCreated={handleWorkoutCreated} />

      {workouts.length === 0 ? (
        <p className="status-msg">Inga workouts ännu. Skapa din första!</p>
      ) : (
        workouts.map((w) => (
          <Link to={`/workouts/${w.id}`} key={w.id}>
            <div className="card">
              <div className="card-title">{w.name}</div>
            </div>
          </Link>
        ))
      )}
    </div>
  );
};

export default WorkoutListPage;
