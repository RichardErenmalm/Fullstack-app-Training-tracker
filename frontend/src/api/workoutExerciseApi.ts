import axios from 'axios';

const API_BASE_URL = 'https://localhost:7026/api';

export const addExerciseToWorkout = async (workoutId: number, exerciseId: number) => {
  await axios.post(`${API_BASE_URL}/workoutExercises`, {
    workoutId,
    exerciseId,
  });
};
