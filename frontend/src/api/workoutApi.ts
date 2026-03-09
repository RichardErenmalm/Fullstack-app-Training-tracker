import api, { OperationResult } from "./api";
import { Workout } from "../types/Workout";

export const getWorkouts = async (): Promise<Workout[]> => {
  const response = await api.get<OperationResult<Workout[]>>("/workouts");

  if (response.data.isSuccess) {
    return response.data.data;
  } else {
    throw new Error(response.data.errorMessage || "Failed to fetch workouts");
  }
};

export const createWorkout = async (workout: { name: string; userId: number }): Promise<Workout> => {
  const response = await api.post<Workout>("/workouts", workout);
  return response.data;
};

export const getWorkoutById = async (id: number): Promise<Workout> => {
  const response = await api.get<Workout>(`/workouts/${id}`);
  return response.data;
};
