namespace WorkoutTracker.API.DTO
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int WorkoutProgramId { get; set; }
        public bool IsActive { get; set; }
        public string? WorkoutProgramName { get; set; }
    }
}