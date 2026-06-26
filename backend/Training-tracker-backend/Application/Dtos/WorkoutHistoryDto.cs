namespace Application.Dtos
{
    public class WorkoutHistoryDto
    {
        public int Id { get; set; }
        public DateTime PerformedAt { get; set; }
        public int? WorkoutId { get; set; }
        public int UserId { get; set; }
    }
}
