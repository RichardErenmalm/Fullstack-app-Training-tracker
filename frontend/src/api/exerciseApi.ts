import axios from 'axios';
import { Exercise } from '../types/Exercise';

const API_BASE_URL = 'https://localhost:7026/api';

export const getExercises = async (): Promise<Exercise[]> => {
  const response = await axios.get(`${API_BASE_URL}/exercises`);
  return response.data.data; // om du använder OperationResult
};
