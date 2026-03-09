import axios from "axios";

const API_BASE_URL = process.env.REACT_APP_API_URL;

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

export default api;
