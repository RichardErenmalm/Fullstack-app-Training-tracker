// src/api/api.ts
import axios from "axios";

// ✅ Make sure the env variable matches this exactly
export const API_BASE_URL = process.env.REACT_APP_API_URL;

console.log("API URL:", API_BASE_URL); // Should print your API base URL

// Create an axios instance for all API calls
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export default api;
