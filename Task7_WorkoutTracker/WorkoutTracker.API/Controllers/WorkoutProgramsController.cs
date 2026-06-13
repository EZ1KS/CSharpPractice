using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.API.Data;
using WorkoutTracker.API.DTO;
using WorkoutTracker.API.Models;

namespace WorkoutTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutProgramsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public WorkoutProgramsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutProgramDto>>> Get()
        {
            var programs = await _context.WorkoutPrograms
                .Select(p => new WorkoutProgramDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    IsActive = p.IsActive
                })
                .ToListAsync();
            return Ok(programs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutProgramDto>> Get(int id)
        {
            var program = await _context.WorkoutPrograms.FindAsync(id);
            if (program == null) return NotFound();
            return new WorkoutProgramDto
            {
                Id = program.Id,
                Name = program.Name,
                Type = program.Type,
                IsActive = program.IsActive
            };
        }

        [HttpPost]
        public async Task<ActionResult<WorkoutProgramDto>> Post(WorkoutProgram program)
        {
            _context.WorkoutPrograms.Add(program);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = program.Id },
                new WorkoutProgramDto { Id = program.Id, Name = program.Name, Type = program.Type, IsActive = program.IsActive });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, WorkoutProgram program)
        {
            if (id != program.Id) return BadRequest();
            _context.Entry(program).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var program = await _context.WorkoutPrograms.FindAsync(id);
            if (program == null) return NotFound();
            _context.WorkoutPrograms.Remove(program);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}