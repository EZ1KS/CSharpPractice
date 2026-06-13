namespace WorkoutTracker.API.DTO
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Minutes { get; set; }
        public string? Notes { get; set; }
        public int ExerciseId { get; set; }
        public string? ExerciseName { get; set; }
        public string? WorkoutProgramName { get; set; }
    }
}