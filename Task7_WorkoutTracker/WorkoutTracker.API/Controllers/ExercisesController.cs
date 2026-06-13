using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.API.Data;
using WorkoutTracker.API.DTO;
using WorkoutTracker.API.Models;

namespace WorkoutTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ExercisesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> Get()
        {
            var exercises = await _context.Exercises
                .Include(e => e.WorkoutProgram)
                .Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    WorkoutProgramId = e.WorkoutProgramId,
                    IsActive = e.IsActive,
                    WorkoutProgramName = e.WorkoutProgram.Name
                })
                .ToListAsync();
            return Ok(exercises);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDto>> Get(int id)
        {
            var exercise = await _context.Exercises
                .Include(e => e.WorkoutProgram)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (exercise == null) return NotFound();
            return new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                WorkoutProgramId = exercise.WorkoutProgramId,
                IsActive = exercise.IsActive,
                WorkoutProgramName = exercise.WorkoutProgram?.Name
            };
        }

        [HttpPost]
        public async Task<ActionResult<ExerciseDto>> Post(Exercise exercise)
        {
            var program = await _context.WorkoutPrograms.FindAsync(exercise.WorkoutProgramId);
            if (program == null) return BadRequest("Программа не найдена");

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            var dto = new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                WorkoutProgramId = exercise.WorkoutProgramId,
                IsActive = exercise.IsActive,
                WorkoutProgramName = program.Name
            };
            return CreatedAtAction(nameof(Get), new { id = exercise.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Exercise exercise)
        {
            if (id != exercise.Id) return BadRequest();
            _context.Entry(exercise).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}