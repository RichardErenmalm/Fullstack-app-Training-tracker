import api, { OperationResult } from "./api";
import { Exercise } from "../types/Exercise";

export const getExercises = async (): Promise<Exercise[]> => {
  const response = await api.get<OperationResult<Exercise[]>>("/exercises");

  if (response.data.isSuccess) {
    return response.data.data;
  } else {
    throw new Error(response.data.errorMessage || "Failed to fetch exercises");
  }
};

export const getExerciseById = async (id: number): Promise<Exercise> => {
  const response = await api.get<Exercise>(`/exercises/${id}`);
  return response.data;
};
