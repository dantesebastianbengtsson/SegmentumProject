using Microsoft.EntityFrameworkCore;
using Segmentum.Data;
using Segmentum.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Allow requests from React dev server
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers(); // Add controllers for API-only project

var app = builder.Build();

// Configure the HTTP request pipeline
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

// Apply CORS middleware before routing
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // Map API controllers directly

// Test endpoints
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

app.Run();
