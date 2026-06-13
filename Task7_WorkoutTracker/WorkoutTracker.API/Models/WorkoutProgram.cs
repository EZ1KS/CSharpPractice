namespace WorkoutTracker.API.Models;

public class WorkoutProgram
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}