using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace WorkoutTracker.API.Models;

public class Activity
{
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Range(1, 1440, ErrorMessage = "Минуты должны быть от 1 до 1440")]
    public int Minutes { get; set; }

    [StringLength(200)]
    public string? Notes { get; set; }

    public int ExerciseId { get; set; }

    [JsonIgnore]
    [ForeignKey("ExerciseId")]
    public Exercise? Exercise { get; set; }
}