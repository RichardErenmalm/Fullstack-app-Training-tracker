import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import WorkoutListPage from "./pages/WorkoutListPage";
import WorkoutDetailPage from "./pages/WorkoutDetailPage";

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<WorkoutListPage />} />
        <Route path="/workouts/:id" element={<WorkoutDetailPage />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;
