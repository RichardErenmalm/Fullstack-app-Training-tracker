import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import WorkoutListPage from './pages/WorkoutListPage';
import WorkoutDetailPage from './pages/WorkoutDetailPage';
import AddExerciseToWorkoutPage from './pages/AddExerciseToWorkoutPage';
import StartWorkoutPage from './pages/StartWorkoutPage';
import MyExercisesPage from './pages/MyExercisesPage';
import ExerciseHistoryPage from './pages/ExerciseHistoryPage';
import './App.css';

function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <Routes>
        <Route path="/" element={<WorkoutListPage />} />
        <Route path="/exercises" element={<MyExercisesPage />} />
        <Route path="/exercise-history/:exerciseId" element={<ExerciseHistoryPage />} />
        <Route path="/workouts/:id" element={<WorkoutDetailPage />} />
        <Route path="/workouts/:id/add-exercise" element={<AddExerciseToWorkoutPage />} />
        <Route path="/workouts/:id/start" element={<StartWorkoutPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
