import React from "react";
import WorkoutList from "../components/WorkoutList";

const WorkoutListPage: React.FC = () => {
  return (
    <div>
      <h1>My Workouts</h1>
      <WorkoutList />
    </div>
  );
};

export default WorkoutListPage;
