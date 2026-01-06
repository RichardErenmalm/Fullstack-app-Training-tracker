import React, { useState } from "react";
import { createWorkout } from "../api/workoutApi";

const CreateWorkoutForm: React.FC = () => {
  const [name, setName] = useState("");
  const [message, setMessage] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setMessage(null);

    try {
      const result = await createWorkout(name);

      if (result.isSuccess) {
        setMessage("Workout created successfully!");
        setName("");
      } else {
        setMessage(result.errorMessage ?? "Failed to create workout");
      }
    } catch (error) {
      setMessage("Something went wrong while creating the workout");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Workout name</label>
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
      </div>

      <button type="submit" disabled={loading}>
        {loading ? "Creating..." : "Create workout"}
      </button>

      {message && <p>{message}</p>}
    </form>
  );
};

export default CreateWorkoutForm;
