using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WorkoutTracker.API.Models;

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int WorkoutProgramId { get; set; }
    public bool IsActive { get; set; } = true;

    [JsonIgnore]
    [ForeignKey("WorkoutProgramId")]
    public WorkoutProgram? WorkoutProgram { get; set; }

    [JsonIgnore]
    public ICollection<Activity>? Activities { get; set; }
}