import { useParams, useNavigate, Link } from 'react-router-dom';
import { useEffect, useState, useCallback } from 'react';
import { getWorkoutExercisesByWorkoutId, saveExerciseHistory, deleteExerciseHistory, createWorkoutHistory, deleteWorkoutHistory } from '../api/workoutExerciseApi';
import { getExerciseById } from '../api/exerciseApi';

type ExerciseSetInput = {
  setNumber: number;
  reps: number;
  weight: number;
  saved: boolean;
  saving: boolean;
  savedHistoryId: number | null;
};

type ExerciseWithSets = {
  exerciseId: number;
  exerciseName: string;
  workoutExerciseId: number;
  sets: ExerciseSetInput[];
};

type SessionState = {
  exercises: ExerciseWithSets[];
  workoutHistoryId: number | null;
};

const STORAGE_KEY_PREFIX = 'workout-session-';

const StartWorkoutPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const storageKey = `${STORAGE_KEY_PREFIX}${id}`;

  const [exercisesWithSets, setExercisesWithSets] = useState<ExerciseWithSets[]>([]);
  const [workoutHistoryId, setWorkoutHistoryId] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const persistState = useCallback((exercises: ExerciseWithSets[], whId: number | null) => {
    const state: SessionState = { exercises, workoutHistoryId: whId };
    sessionStorage.setItem(storageKey, JSON.stringify(state));
  }, [storageKey]);

  useEffect(() => {
    const init = async () => {
      try {
        if (!id) return;

        // Try restoring from sessionStorage
        const stored = sessionStorage.getItem(storageKey);
        if (stored) {
          const parsed = JSON.parse(stored);
          // Handle both old format (array) and new format (object with exercises)
          if (parsed && parsed.exercises && parsed.workoutHistoryId) {
            setExercisesWithSets(parsed.exercises);
            setWorkoutHistoryId(parsed.workoutHistoryId);
            setLoading(false);
            return;
          }
          // Invalid/old format — clear and start fresh
          sessionStorage.removeItem(storageKey);
        }

        // Create a new WorkoutHistory for this session
        const wh = await createWorkoutHistory(Number(id), 1);

        const workoutExercises = await getWorkoutExercisesByWorkoutId(Number(id));

        const exercises: ExerciseWithSets[] = await Promise.all(
          workoutExercises.map(async (we) => {
            const exercise = await getExerciseById(we.exerciseId);
            const totalSets = we.sets || 3;
            const sets: ExerciseSetInput[] = Array.from({ length: totalSets }, (_, i) => ({
              setNumber: i + 1,
              reps: 0,
              weight: 0,
              saved: false,
              saving: false,
              savedHistoryId: null,
            }));
            return {
              exerciseId: we.exerciseId,
              exerciseName: exercise.name,
              workoutExerciseId: we.id,
              sets,
            };
          })
        );

        setExercisesWithSets(exercises);
        setWorkoutHistoryId(wh.id);
        persistState(exercises, wh.id);
      } catch (err: any) {
        console.error(err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    init();
  }, [id, storageKey, persistState]);

  const updateSet = (
    exerciseId: number,
    setNumber: number,
    field: 'reps' | 'weight',
    value: number
  ) => {
    setExercisesWithSets((prev) => {
      const updated = prev.map((ex) =>
        ex.exerciseId === exerciseId
          ? {
              ...ex,
              sets: ex.sets.map((s) =>
                s.setNumber === setNumber ? { ...s, [field]: value } : s
              ),
            }
          : ex
      );
      persistState(updated, workoutHistoryId);
      return updated;
    });
  };

  const setSaving = (exerciseId: number, setNumber: number, saving: boolean) => {
    setExercisesWithSets((prev) =>
      prev.map((e) =>
        e.exerciseId === exerciseId
          ? { ...e, sets: e.sets.map((s) => s.setNumber === setNumber ? { ...s, saving } : s) }
          : e
      )
    );
  };

  const toggleSetDone = async (ex: ExerciseWithSets, set: ExerciseSetInput) => {
    if (!workoutHistoryId || set.saving) return;

    // Lock immediately
    setSaving(ex.exerciseId, set.setNumber, true);

    if (set.saved && set.savedHistoryId) {
      try {
        await deleteExerciseHistory(set.savedHistoryId);
        setExercisesWithSets((prev) => {
          const updated = prev.map((e) =>
            e.exerciseId === ex.exerciseId
              ? {
                  ...e,
                  sets: e.sets.map((s) =>
                    s.setNumber === set.setNumber
                      ? { ...s, saved: false, saving: false, savedHistoryId: null }
                      : s
                  ),
                }
              : e
          );
          persistState(updated, workoutHistoryId);
          return updated;
        });
      } catch (err) {
        console.error(err);
        setSaving(ex.exerciseId, set.setNumber, false);
        alert('Kunde inte ta bort setet');
      }
    } else {
      try {
        const result = await saveExerciseHistory({
          exerciseId: ex.exerciseId,
          reps: set.reps,
          weightKg: set.weight,
          setNumber: set.setNumber,
          userId: 1,
          workoutExerciseId: ex.workoutExerciseId,
          workoutHistoryId,
        });

        if (!result.isSuccess) {
          setSaving(ex.exerciseId, set.setNumber, false);
          return;
        }

        const savedId = result.data?.id ?? null;

        setExercisesWithSets((prev) => {
          const updated = prev.map((e) =>
            e.exerciseId === ex.exerciseId
              ? {
                  ...e,
                  sets: e.sets.map((s) =>
                    s.setNumber === set.setNumber
                      ? { ...s, saved: true, saving: false, savedHistoryId: savedId }
                      : s
                  ),
                }
              : e
          );
          persistState(updated, workoutHistoryId);
          return updated;
        });
      } catch (err) {
        console.error(err);
        setSaving(ex.exerciseId, set.setNumber, false);
        alert('Kunde inte spara setet');
      }
    }
  };

  const finishWorkout = async () => {
    if (!workoutHistoryId) return;

    const unsaved: Promise<any>[] = [];
    for (const ex of exercisesWithSets) {
      for (const s of ex.sets) {
        if (!s.saved && (s.reps > 0 || s.weight > 0)) {
          unsaved.push(
            saveExerciseHistory({
              exerciseId: ex.exerciseId,
              reps: s.reps,
              weightKg: s.weight,
              setNumber: s.setNumber,
              userId: 1,
              workoutExerciseId: ex.workoutExerciseId,
              workoutHistoryId,
            })
          );
        }
      }
    }

    try {
      await Promise.all(unsaved);
      sessionStorage.removeItem(storageKey);
      navigate(`/workouts/${id}`);
    } catch {
      alert('Kunde inte spara alla sets');
    }
  };

  const cancelWorkout = async () => {
    if (!workoutHistoryId) {
      sessionStorage.removeItem(storageKey);
      navigate(`/workouts/${id}`);
      return;
    }

    // Delete all saved exercise histories
    const savedIds = exercisesWithSets
      .flatMap((ex) => ex.sets)
      .filter((s) => s.saved && s.savedHistoryId)
      .map((s) => s.savedHistoryId!);

    try {
      await Promise.all(savedIds.map((hid) => deleteExerciseHistory(hid)));
      // Delete the workout history itself
      await deleteWorkoutHistory(workoutHistoryId);
      sessionStorage.removeItem(storageKey);
      navigate(`/workouts/${id}`);
    } catch (err) {
      console.error(err);
      alert('Kunde inte avbryta workoutet');
    }
  };

  if (loading) return <div className="page"><p className="status-msg">Laddar workout...</p></div>;
  if (error) return <div className="page"><p className="error-msg">{error}</p></div>;

  const allDone = exercisesWithSets.every((ex) => ex.sets.every((s) => s.saved));

  return (
    <div className="page">
      <Link to={`/workouts/${id}`} className="back-link" onClick={() => sessionStorage.removeItem(storageKey)}>&larr; Tillbaka</Link>
      <h2>Workout</h2>

      {exercisesWithSets.map((ex) => (
        <div className="exercise-section" key={ex.exerciseId}>
          <div className="exercise-section-header">
            <h3>{ex.exerciseName}</h3>
            <button
              className="btn btn-secondary btn-sm"
              onClick={() => navigate(`/exercise-history/${ex.exerciseId}`, { state: { from: `/workouts/${id}/start` } })}
            >
              Historik
            </button>
          </div>

          <div className="set-header-check">
            <div></div>
            <div>Set</div>
            <div>Reps</div>
            <div>Vikt (kg)</div>
          </div>

          {ex.sets.map((s) => (
            <div className={`set-row-check ${s.saved ? 'set-done' : ''}`} key={s.setNumber}>
              <div>
                <input
                  type="checkbox"
                  className="set-checkbox"
                  checked={s.saved || s.saving}
                  disabled={s.saving}
                  onChange={() => toggleSetDone(ex, s)}
                />
              </div>
              <div className="set-number">{s.setNumber}</div>
              <input
                className="set-input"
                type="number"
                placeholder="0"
                value={s.reps || ''}
                disabled={s.saved}
                onChange={(e) =>
                  updateSet(ex.exerciseId, s.setNumber, 'reps', Number(e.target.value))
                }
              />
              <input
                className="set-input"
                type="number"
                placeholder="0"
                value={s.weight || ''}
                disabled={s.saved}
                onChange={(e) =>
                  updateSet(ex.exerciseId, s.setNumber, 'weight', Number(e.target.value))
                }
              />
            </div>
          ))}
        </div>
      ))}

      <div className="btn-group" style={{ marginTop: '0.5rem' }}>
        <button className="btn btn-success" onClick={finishWorkout} style={{ flex: 1 }}>
          {allDone ? 'Klar' : 'Spara & Klar'}
        </button>
        <button className="btn btn-danger" onClick={cancelWorkout}>
          Avbryt
        </button>
      </div>
    </div>
  );
};

export default StartWorkoutPage;
