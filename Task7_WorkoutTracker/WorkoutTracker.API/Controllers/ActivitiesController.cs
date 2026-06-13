using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.API.Data;
using WorkoutTracker.API.DTO;
using WorkoutTracker.API.Models;

namespace WorkoutTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ActivitiesController(AppDbContext context) => _context = context;

        // GET: api/activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetAll()
        {
            var activities = await _context.Activities
                .Include(a => a.Exercise)
                    .ThenInclude(e => e.WorkoutProgram)
                .OrderByDescending(a => a.Date)
                .Select(a => new ActivityDto
                {
                    Id = a.Id,
                    Date = a.Date,
                    Minutes = a.Minutes,
                    Notes = a.Notes,
                    ExerciseId = a.ExerciseId,
                    ExerciseName = a.Exercise.Name,
                    WorkoutProgramName = a.Exercise.WorkoutProgram.Name
                })
                .ToListAsync();
            return Ok(activities);
        }

        // GET: api/activities/date?date=2026-06-13
        [HttpGet("date")]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetByDate([FromQuery] DateTime date)
        {
            var activities = await _context.Activities
                .Where(a => a.Date.Date == date.Date)
                .Include(a => a.Exercise)
                    .ThenInclude(e => e.WorkoutProgram)
                .Select(a => new ActivityDto
                {
                    Id = a.Id,
                    Date = a.Date,
                    Minutes = a.Minutes,
                    Notes = a.Notes,
                    ExerciseId = a.ExerciseId,
                    ExerciseName = a.Exercise.Name,
                    WorkoutProgramName = a.Exercise.WorkoutProgram.Name
                })
                .ToListAsync();
            return Ok(activities);
        }

        // GET: api/activities/month?year=2026&month=6
        [HttpGet("month")]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetByMonth(int year, int month)
        {
            
            if (year < 2000 || year > DateTime.Now.Year + 10)
                return BadRequest("Год должен быть между 2000 и " + (DateTime.Now.Year + 10));
            if (month < 1 || month > 12)
                return BadRequest("Месяц должен быть от 1 до 12");

            
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            
            var activities = await _context.Activities
                .Where(a => a.Date >= startDate && a.Date < endDate)
                .Include(a => a.Exercise)
                    .ThenInclude(e => e.WorkoutProgram)
                .Select(a => new ActivityDto
                {
                    Id = a.Id,
                    Date = a.Date,
                    Minutes = a.Minutes,
                    Notes = a.Notes,
                    ExerciseId = a.ExerciseId,
                    ExerciseName = a.Exercise.Name,
                    WorkoutProgramName = a.Exercise.WorkoutProgram.Name
                })
                .ToListAsync();

            return Ok(activities);
        }

        // GET: api/activities/sticker?date=2026-06-13
        [HttpGet("sticker")]
        public async Task<ActionResult<object>> GetSticker([FromQuery] DateTime date)
        {
            var totalMinutes = await _context.Activities
                .Where(a => a.Date.Date == date.Date)
                .SumAsync(a => a.Minutes);

            string color;
            if (totalMinutes < 30) color = "yellow";
            else if (totalMinutes <= 90) color = "green";
            else color = "red";

            return Ok(new { date = date.Date, totalMinutes, sticker = color });
        }

        // GET: api/activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetById(int id)
        {
            var a = await _context.Activities
                .Include(a => a.Exercise)
                    .ThenInclude(e => e.WorkoutProgram)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (a == null) return NotFound();
            return new ActivityDto
            {
                Id = a.Id,
                Date = a.Date,
                Minutes = a.Minutes,
                Notes = a.Notes,
                ExerciseId = a.ExerciseId,
                ExerciseName = a.Exercise.Name,
                WorkoutProgramName = a.Exercise.WorkoutProgram.Name
            };
        }

        // POST: api/activities
        [HttpPost]
        public async Task<ActionResult<ActivityDto>> Post(Activity activity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exercise = await _context.Exercises.FindAsync(activity.ExerciseId);
            if (exercise == null) return BadRequest("Упражнение не найдено");
            if (!exercise.IsActive) return BadRequest("Нельзя добавить активность к неактивному упражнению");

            var date = activity.Date.Date;
            var existingTotal = await _context.Activities
                .Where(a => a.Date.Date == date)
                .SumAsync(a => a.Minutes);
            if (existingTotal + activity.Minutes > 1440)
                return BadRequest("Суммарное время активностей за день не может превышать 1440 минут");

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            // загружаем навигационные свойства для DTO
            await _context.Entry(activity).Reference(a => a.Exercise).Query().Include(e => e.WorkoutProgram).LoadAsync();
            var dto = new ActivityDto
            {
                Id = activity.Id,
                Date = activity.Date,
                Minutes = activity.Minutes,
                Notes = activity.Notes,
                ExerciseId = activity.ExerciseId,
                ExerciseName = activity.Exercise.Name,
                WorkoutProgramName = activity.Exercise.WorkoutProgram.Name
            };
            return CreatedAtAction(nameof(GetById), new { id = activity.Id }, dto);
        }

        // PUT: api/activities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Activity updated)
        {
            if (id != updated.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _context.Activities
                .Include(a => a.Exercise)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (existing == null) return NotFound();

            // Если пытаются сменить упражнение, проверяем условия
            if (existing.ExerciseId != updated.ExerciseId)
            {
                var currentExercise = await _context.Exercises.FindAsync(existing.ExerciseId);
                if (currentExercise != null && !currentExercise.IsActive)
                    return BadRequest("Нельзя изменить упражнение, так как текущее упражнение неактивно.");

                var newExercise = await _context.Exercises.FindAsync(updated.ExerciseId);
                if (newExercise == null || !newExercise.IsActive)
                    return BadRequest("Новое упражнение не существует или неактивно.");
            }

            // Проверка лимита минут (исключая текущую запись)
            var date = updated.Date.Date;
            var totalWithoutCurrent = await _context.Activities
                .Where(a => a.Date.Date == date && a.Id != id)
                .SumAsync(a => a.Minutes);
            if (totalWithoutCurrent + updated.Minutes > 1440)
                return BadRequest("Превышен лимит 1440 минут в день");

            existing.Date = updated.Date;
            existing.Minutes = updated.Minutes;
            existing.Notes = updated.Notes;
            existing.ExerciseId = updated.ExerciseId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return NotFound();
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}