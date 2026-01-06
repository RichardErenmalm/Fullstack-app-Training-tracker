 import React, { useEffect, useState } from "react";
import { getWorkouts } from "../api/workoutApi";

type Workout = {
  id: number;
  name: string;
};

const WorkoutList: React.FC = () => {
  const [workouts, setWorkouts] = useState<Workout[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchWorkouts = async () => {
      try {
        const result = await getWorkouts();

        if (result.isSuccess) {
          setWorkouts(result.data);
        } else {
          setError(result.errorMessage ?? "Failed to load workouts");
        }
      } catch {
        setError("Something went wrong");
      } finally {
        setLoading(false);
      }
    };

    fetchWorkouts();
  }, []);

  if (loading) return <p>Loading workouts...</p>;
  if (error) return <p>{error}</p>;

  return (
    <ul>
      {workouts.map((w) => (
        <li key={w.id}>{w.name}</li>
      ))}
    </ul>
  );
};

export default WorkoutList;
