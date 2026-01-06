import React, { useState } from 'react';
import { createWorkout } from '../api/workoutApi';
import { Workout } from '../types/Workout';

interface Props {
  onWorkoutCreated: (newWorkout: Workout) => void;
}

const CreateWorkoutForm: React.FC<Props> = ({ onWorkoutCreated }) => {
  const [name, setName] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const userId = 1; // temporärt, vi kan fixa inloggning senare

const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();
  if (!name) return;

  try {
    const newWorkout = await createWorkout({
      name,
      userId: 1 // 👈 TEMPORÄRT, tills auth finns
    });

    onWorkoutCreated(newWorkout);
    setName('');
  } catch (err) {
    console.error('Error creating workout', err);
  }
};



  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Workout name"
        value={name}
        onChange={(e) => setName(e.target.value)}
      />
      <button type="submit" disabled={loading}>
        {loading ? 'Skapar...' : 'Skapa Workout'}
      </button>
      {error && <p style={{ color: 'red' }}>{error}</p>}
    </form>
  );
};

export default CreateWorkoutForm;
