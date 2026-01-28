// src/api/workoutExerciseApi.ts
import axios from "axios";

// Base URL från .env
const API_BASE_URL = process.env.REACT_APP_API_URL!;
console.log("API_BASE_URL:", API_BASE_URL);

// Axios-instans
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export interface OperationResult<T> {
  isSuccess: boolean;
  data: T;
  errorMessage?: string;
}

// Lägg till exercise till en workout
export const addExerciseToWorkout = async (workoutId: number, exerciseId: number): Promise<void> => {
  try {
    const response = await api.post("/workoutExercises", {
      workoutId,
      exerciseId,
    });

    if (!response.data.isSuccess) {
      console.error("Failed to add exercise to workout:", response.data.errorMessage);
      throw new Error(response.data.errorMessage);
    }
  } catch (err) {
    console.error("Error in addExerciseToWorkout:", err);
    throw err;
  }
};
