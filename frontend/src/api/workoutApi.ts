import axios from 'axios';
import { Workout } from '../types/Workout';

const API_BASE_URL = 'https://localhost:7026/api';

export interface OperationResult<T> {
  isSuccess: boolean;
  data: T;
  errorMessage?: string;
}



// Hämta alla workouts
export const getWorkouts = async (): Promise<Workout[]> => {
  const response = await axios.get<OperationResult<Workout[]>>(`${API_BASE_URL}/workouts`);
  
  if (response.data.isSuccess) {
    return response.data.data; // <-- själva arrayen
  } else {
    console.error('Failed to fetch workouts:', response.data.errorMessage);
    return []; // fallback om något går fel
  }
};

// Skapa en ny workout
export const createWorkout = async (workout: { name: string; userId: number }): Promise<Workout> => {
  const response = await axios.post<Workout>(`${API_BASE_URL}/workouts`, workout);
  return response.data;
};

// Hämta en workout by id
export const getWorkoutById = async (id: number): Promise<Workout> => {
  const response = await axios.get<Workout>(`${API_BASE_URL}/workouts/${id}`);
  return response.data;
};
