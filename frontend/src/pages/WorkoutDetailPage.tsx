import { useParams, useNavigate, Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { getWorkoutById, deleteWorkout } from '../api/workoutApi';
import { getWorkoutExercisesByWorkoutId } from '../api/workoutExerciseApi';
import { getExerciseById } from '../api/exerciseApi';
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
  const [confirmDelete, setConfirmDelete] = useState(false);
  const [deleting, setDeleting] = useState(false);

  useEffect(() => {
    const fetchWorkoutAndExercises = async () => {
      try {
        if (!id) {
          setError('Ogiltigt workout-id');
          return;
        }

        const workoutData = await getWorkoutById(Number(id));
        setWorkout(workoutData);

        const exercisesData = await getWorkoutExercisesByWorkoutId(Number(id));

        const exercisesWithNames: WorkoutExerciseWithName[] = await Promise.all(
          exercisesData.map(async (we) => {
            const exercise = await getExerciseById(we.exerciseId);
            return { ...we, exercise };
          })
        );

        setWorkoutExercises(exercisesWithNames);
      } catch (err: any) {
        console.error(err);
        setError(err.message || `Kunde inte hämta workout med id ${id}`);
      } finally {
        setLoading(false);
      }
    };

    fetchWorkoutAndExercises();
  }, [id]);

  if (loading) return <div className="page"><p className="status-msg">Laddar workout...</p></div>;
  if (error) return <div className="page"><p className="error-msg">{error}</p></div>;
  if (!workout) return <div className="page"><p className="status-msg">Workout hittades inte</p></div>;

  return (
    <div className="page">
      <Link to="/" className="back-link">&larr; Tillbaka</Link>
      <h2>{workout.name}</h2>

      <div className="btn-group">
        <button className="btn btn-primary" onClick={() => navigate(`/workouts/${workout.id}/add-exercise`)}>
          Lägg till övning
        </button>
        <button className="btn btn-success" onClick={() => navigate(`/workouts/${workout.id}/start`)}>
          Starta workout
        </button>
      </div>

      <hr className="divider" />

      <h3>Övningar</h3>
      {workoutExercises.length === 0 ? (
        <p className="status-msg">Inga övningar tillagda ännu.</p>
      ) : (
        workoutExercises.map((we) => (
          <div className="card" key={we.id}>
            <div className="card-title">{we.exercise?.name}</div>
            <div className="card-subtitle">
              {we.sets} sets &middot; {we.reps ?? '–'} reps &middot; {we.weight ?? 0} kg
            </div>
          </div>
        ))
      )}

      <hr className="divider" />

      {!confirmDelete ? (
        <button className="btn btn-danger-outline btn-sm" onClick={() => setConfirmDelete(true)}>
          Delete workout
        </button>
      ) : (
        <div className="confirm-delete">
          <p className="confirm-delete-text">This action cannot be undone. Are you sure?</p>
          <div className="btn-group">
            <button
              className="btn btn-danger btn-sm"
              disabled={deleting}
              onClick={async () => {
                setDeleting(true);
                try {
                  await deleteWorkout(workout.id);
                  navigate('/');
                } catch (err) {
                  console.error(err);
                  alert('Could not delete workout');
                  setDeleting(false);
                }
              }}
            >
              {deleting ? 'Deleting...' : 'Yes, delete'}
            </button>
            <button className="btn btn-secondary btn-sm" onClick={() => setConfirmDelete(false)}>
              Cancel
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default WorkoutDetailPage;
