import api, { OperationResult } from "./api";
import { WorkoutExercise } from "../types/WorkoutExercise";

export const getWorkoutExercisesByWorkoutId = async (workoutId: number): Promise<WorkoutExercise[]> => {
  const response = await api.get<OperationResult<WorkoutExercise[]>>(`/workoutExercises/workout/${workoutId}`);

  if (response.data.isSuccess) {
    return response.data.data;
  } else {
    throw new Error(response.data.errorMessage || "Failed to fetch workout exercises");
  }
};

export const addExerciseToWorkout = async (workoutId: number, exerciseId: number): Promise<void> => {
  const response = await api.post<OperationResult<unknown>>("/workoutExercises", {
    workoutId,
    exerciseId,
  });

  if (!response.data.isSuccess) {
    throw new Error(response.data.errorMessage || "Failed to add exercise to workout");
  }
};

export const saveExerciseHistory = async (entry: {
  exerciseId: number;
  reps: number;
  weightKg: number;
  setNumber: number;
  userId: number;
  workoutExerciseId: number;
  workoutHistoryId: number;
}): Promise<OperationResult<{ id: number }>> => {
  const response = await api.post<OperationResult<{ id: number }>>("/exerciseHistory", entry);
  return response.data;
};

export const deleteExerciseHistory = async (id: number): Promise<void> => {
  await api.delete(`/exerciseHistory/${id}`, { data: { id } });
};

export const createWorkoutHistory = async (workoutId: number, userId: number): Promise<{ id: number }> => {
  const response = await api.post<OperationResult<{ id: number }>>("/workoutHistory", {
    workoutId,
    userId,
  });
  if (response.data.isSuccess) {
    return response.data.data;
  }
  throw new Error(response.data.errorMessage || "Failed to create workout history");
};

export const deleteWorkoutHistory = async (id: number): Promise<void> => {
  await api.delete(`/workoutHistory/${id}`, { data: { id } });
};
