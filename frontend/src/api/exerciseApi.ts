// src/api/exerciseApi.ts
import axios from "axios";
import { Exercise } from "../types/Exercise";

// Base URL från .env
const API_BASE_URL = process.env.REACT_APP_API_URL!;
console.log("API_BASE_URL:", API_BASE_URL);

// Axios-instans för alla requests
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

// Hämta alla exercises
export const getExercises = async (): Promise<Exercise[]> => {
  const response = await api.get<OperationResult<Exercise[]>>("/exercises");

  if (response.data.isSuccess) {
    return response.data.data;
  } else {
    console.error("Failed to fetch exercises:", response.data.errorMessage);
    return [];
  }
};

// Hämta en exercise by id
export const getExerciseById = async (id: number): Promise<Exercise> => {
  const response = await api.get<OperationResult<Exercise>>(`/exercises/${id}`);

  if (response.data.isSuccess) {
    return response.data.data;
  } else {
    throw new Error(response.data.errorMessage || `Failed to fetch exercise with id ${id}`);
  }
};
