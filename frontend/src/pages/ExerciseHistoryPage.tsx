import React, { useEffect, useState } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import { getExerciseById } from '../api/exerciseApi';
import api, { OperationResult } from '../api/api';

type ExerciseHistoryEntry = {
  id: number;
  name: string;
  weightKg: number;
  reps: number;
  setNumber: number;
  exerciseId: number;
  workoutExerciseId: number | null;
  workoutHistoryId: number | null;
};

type WorkoutHistoryInfo = {
  id: number;
  performedAt: string;
  workoutId: number | null;
};

const ExerciseHistoryPage: React.FC = () => {
  const { exerciseId } = useParams<{ exerciseId: string }>();
  const navigate = useNavigate();
  const location = useLocation();

  const [exerciseName, setExerciseName] = useState('');
  const [groups, setGroups] = useState<{ workoutHistory: WorkoutHistoryInfo; entries: ExerciseHistoryEntry[] }[]>([]);
  const [loading, setLoading] = useState(true);

  const backPath = (location.state as any)?.from || '/exercises';

  useEffect(() => {
    const fetch = async () => {
      try {
        const exercise = await getExerciseById(Number(exerciseId));
        setExerciseName(exercise.name);

        // Fetch all exercise histories
        const histResponse = await api.get<OperationResult<ExerciseHistoryEntry[]>>('/exercisehistory');
        const allHistory = histResponse.data.isSuccess ? histResponse.data.data : [];
        const filtered = allHistory.filter((h) => h.exerciseId === Number(exerciseId));

        // Fetch all workout histories
        const whResponse = await api.get<OperationResult<WorkoutHistoryInfo[]>>('/workouthistory');
        const allWh = whResponse.data.isSuccess ? whResponse.data.data : [];
        const whMap = new Map(allWh.map((wh) => [wh.id, wh]));

        // Group by workoutHistoryId (use 0 for null/detached entries)
        const groupMap = new Map<number, ExerciseHistoryEntry[]>();
        for (const entry of filtered) {
          const key = entry.workoutHistoryId ?? 0;
          if (!groupMap.has(key)) groupMap.set(key, []);
          groupMap.get(key)!.push(entry);
        }

        const grouped = Array.from(groupMap.entries())
          .map(([whId, entries]) => ({
            workoutHistory: whMap.get(whId) || { id: whId, performedAt: '', workoutId: null },
            entries: entries.sort((a, b) => a.setNumber - b.setNumber),
          }))
          .sort((a, b) => new Date(b.workoutHistory.performedAt).getTime() - new Date(a.workoutHistory.performedAt).getTime());

        setGroups(grouped);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    fetch();
  }, [exerciseId]);

  if (loading) return <div className="page"><p className="status-msg">Laddar historik...</p></div>;

  return (
    <div className="page">
      <button className="back-link" onClick={() => navigate(backPath)}>&larr; Tillbaka</button>
      <h2>{exerciseName}</h2>

      {groups.length === 0 ? (
        <p className="status-msg">Ingen historik finns för {exerciseName} ännu.</p>
      ) : (
        <div className="history-list">
          {groups.map((group) => (
            <div className="history-group" key={group.workoutHistory.id}>
              <div className="history-group-title">
                {group.workoutHistory.performedAt
                  ? new Date(group.workoutHistory.performedAt).toLocaleDateString('sv-SE', {
                      year: 'numeric',
                      month: 'long',
                      day: 'numeric',
                    })
                  : 'Okänt datum'}
              </div>
              <div className="set-header">
                <div>Set</div>
                <div>Reps</div>
                <div>Vikt (kg)</div>
              </div>
              {group.entries.map((entry) => (
                <div className="set-row" key={entry.id}>
                  <div className="set-number">{entry.setNumber}</div>
                  <div>{entry.reps}</div>
                  <div>{entry.weightKg} kg</div>
                </div>
              ))}
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ExerciseHistoryPage;
