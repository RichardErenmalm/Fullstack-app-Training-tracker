import { BrowserRouter, Routes, Route } from 'react-router-dom';
import WorkoutListPage from './pages/WorkoutListPage';
import WorkoutDetailPage from './pages/WorkoutDetailPage';
import AddExerciseToWorkoutPage from './pages/AddExerciseToWorkoutPage';
import StartWorkoutPage from './pages/StartWorkoutPage';


function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<WorkoutListPage />} />
        <Route path="/workouts/:id" element={<WorkoutDetailPage />} />
        <Route path="/workouts/:id/add-exercise" element={<AddExerciseToWorkoutPage />} />
        <Route path="/workouts/:id/start" element={<StartWorkoutPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

