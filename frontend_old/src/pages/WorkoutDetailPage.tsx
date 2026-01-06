import React from "react";
import { useParams } from "react-router-dom";

const WorkoutDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();

  return (
    <div>
      <h2>Workout Detail - ID: {id}</h2>
      <p>Här kommer workout-informationen att visas</p>
    </div>
  );
};

export default WorkoutDetailPage;
