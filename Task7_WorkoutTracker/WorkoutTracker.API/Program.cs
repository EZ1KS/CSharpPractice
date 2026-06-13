using Microsoft.EntityFrameworkCore;
using WorkoutTracker.API.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    { 
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Автоматически накатить миграции и залить тестовые данные
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    // Seed тестовых данных (если БД пустая)
    if (!dbContext.WorkoutPrograms.Any())
    {
        var programs = new[]
        {
            new WorkoutTracker.API.Models.WorkoutProgram { Name = "Силовая", Type = "Силовая", IsActive = true },
            new WorkoutTracker.API.Models.WorkoutProgram { Name = "Кардио", Type = "Кардио", IsActive = true },
            new WorkoutTracker.API.Models.WorkoutProgram { Name = "Йога", Type = "Растяжка", IsActive = false }
        };
        dbContext.WorkoutPrograms.AddRange(programs);
        dbContext.SaveChanges();

        var exercises = new[]
        {
            new WorkoutTracker.API.Models.Exercise { Name = "Приседания", WorkoutProgramId = 1, IsActive = true },
            new WorkoutTracker.API.Models.Exercise { Name = "Жим лёжа", WorkoutProgramId = 1, IsActive = true },
            new WorkoutTracker.API.Models.Exercise { Name = "Бег 5 км", WorkoutProgramId = 2, IsActive = true },
            new WorkoutTracker.API.Models.Exercise { Name = "Собака мордой вниз", WorkoutProgramId = 3, IsActive = false }
        };
        dbContext.Exercises.AddRange(exercises);
        dbContext.SaveChanges();

        var activities = new[]
        {
            new WorkoutTracker.API.Models.Activity { Date = new DateTime(2026, 6, 13), Minutes = 45, Notes = "Отлично позанимался", ExerciseId = 1 },
            new WorkoutTracker.API.Models.Activity { Date = new DateTime(2026, 6, 13), Minutes = 30, Notes = "Бег", ExerciseId = 3 },
            new WorkoutTracker.API.Models.Activity { Date = new DateTime(2026, 6, 12), Minutes = 20, Notes = "Йога", ExerciseId = 4 }
        };
        dbContext.Activities.AddRange(activities);
        dbContext.SaveChanges();
    }
}

app.Run();