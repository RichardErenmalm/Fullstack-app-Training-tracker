import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:5001/api",
  headers: {
    "Content-Type": "application/json",
  },
});

export const createWorkout = async (name: string) => {
  const response = await api.post("/workouts", {
    name: name,
  });

  return response.data;
};

export const getWorkouts = async () => {
  const response = await api.get("/workouts");
  return response.data;
};


 