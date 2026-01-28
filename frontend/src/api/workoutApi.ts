// src/api/workoutApi.ts
import axios from "axios";
import { Workout } from "../types/Workout";

// ✅ Använd samma env-variabel som du definierat i .env
const API_BASE_URL = process.env.REACT_APP_API_URL!;
console.log("API_BASE_URL:", API_BASE_URL);

// Optional: skapa en axios-instans för alla requests
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

// Hämta alla workouts
export const getWorkouts = async (): Promise<Workout[]> => {
  const response = await api.get<OperationResult<Workout[]>>("/workouts");

  if (response.data.isSuccess) {
    return response.data.data; // själva arrayen
  } else {
    console.error("Failed to fetch workouts:", response.data.errorMessage);
    return []; // fallback om något går fel
  }
};

// Skapa en ny workout
export const createWorkout = async (workout: { name: string; userId: number }): Promise<Workout> => {
  const response = await api.post<Workout>("/workouts", workout);
  return response.data;
};

// Hämta en workout by id
export const getWorkoutById = async (id: number): Promise<Workout> => {
  const response = await api.get<Workout>(`/workouts/${id}`);
  return response.data;
};
