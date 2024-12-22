using Microsoft.EntityFrameworkCore;
using Segmentum.Data;
using Segmentum.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(); // Add controllers for API-only project

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Enforce HTTPS in production
}

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        try
        {
            Console.WriteLine("Testing database connection...");
            context.Database.Migrate(); // Applies migrations
            Console.WriteLine("Database connection successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection failed: {ex.Message}");
        }
    }
}


// Testing create habit
app.MapGet("/test/create-habit", async (AppDbContext context) =>
{
    var habit = new Habit
    {
        Name = "Test Habit",
        Description = "This is a test habit",
        Status = "Succeeded",
        SegmentId = 1 // Make sure a valid SegmentId exists
    };

    context.Habits.Add(habit);
    await context.SaveChangesAsync();

    return Results.Ok(habit);
});

// Testing delete habit
app.MapGet("/test/delete-habit/{id}", async (int id, AppDbContext context) =>
{
    var habit = await context.Habits.FindAsync(id);
    if (habit == null)
    {
        return Results.NotFound($"No habit found with ID {id}");
    }

    context.Habits.Remove(habit);
    await context.SaveChangesAsync();

    return Results.Ok($"Deleted habit with ID {id}");
});

// Test create segment
app.MapGet("/test/create-segment", async (AppDbContext context) =>
{
    var segment = new Segment
    {
        Name = "Test Segment",
        StartDate = DateTime.UtcNow,
        EndDate = DateTime.UtcNow.AddDays(14)
    };

    context.Segments.Add(segment);
    await context.SaveChangesAsync();

    return Results.Ok(segment);
});

// Test delete segment
app.MapGet("/test/delete-segment/{id}", async (int id, AppDbContext context) =>
{
    var segment = await context.Segments.FindAsync(id);
    if (segment == null)
    {
        return Results.NotFound($"No segment found with ID {id}.");
    }

    context.Segments.Remove(segment);
    await context.SaveChangesAsync();

    return Results.Ok($"Segment with ID {id} deleted.");
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // Map API controllers directly
app.Run();
