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

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name) return;

    setLoading(true);
    setError(null);

    try {
      const newWorkout = await createWorkout({
        name,
        userId: 1,
      });

      onWorkoutCreated(newWorkout);
      setName('');
    } catch (err: any) {
      console.error('Error creating workout', err);
      setError(err.message || 'Kunde inte skapa workout');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="form-row">
        <input
          className="input"
          type="text"
          placeholder="Namn på workout..."
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <button className="btn btn-primary" type="submit" disabled={loading}>
          {loading ? 'Skapar...' : 'Skapa Workout'}
        </button>
      </div>
      {error && <p className="error-msg">{error}</p>}
    </form>
  );
};

export default CreateWorkoutForm;
