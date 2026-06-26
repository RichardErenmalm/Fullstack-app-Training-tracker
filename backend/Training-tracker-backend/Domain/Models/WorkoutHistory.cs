namespace Domain.Models
{
    public class WorkoutHistory
    {
        public int Id { get; set; }

        public DateTime PerformedAt { get; set; } = DateTime.Now;

        public int? WorkoutId { get; set; }
        public Workout? Workout { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<ExerciseHistory> ExerciseHistories { get; set; } = new List<ExerciseHistory>();
    }
}
