using Microsoft.EntityFrameworkCore;
using TasksApi.Data;
using TasksApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDb>(o => o.UseSqlite("Data Source=tasks.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: dev frontend (http://localhost:3000)
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
  p.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod()
));

var app = builder.Build();
app.UseCors();

if (app.Environment.IsDevelopment()) {
  app.UseSwagger(); app.UseSwaggerUI();
}

app.MapGet("/api/tasks", async (AppDb db) => await db.Tasks.ToListAsync());
app.MapPost("/api/tasks", async (AppDb db, TaskItem t) => { db.Tasks.Add(t); await db.SaveChangesAsync(); return Results.Created($"/api/tasks/{t.Id}", t); });
app.MapPut("/api/tasks/{id}", async (AppDb db, int id, TaskItem input) => {
  var t = await db.Tasks.FindAsync(id); if (t is null) return Results.NotFound();
  t.Title = input.Title; t.Done = input.Done; await db.SaveChangesAsync(); return Results.Ok(t);
});
app.MapDelete("/api/tasks/{id}", async (AppDb db, int id) => {
  var t = await db.Tasks.FindAsync(id); if (t is null) return Results.NotFound();
  db.Tasks.Remove(t); await db.SaveChangesAsync(); return Results.NoContent();
});

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDb>();
db.Database.Migrate();
if (!db.Tasks.Any()) {
  db.Tasks.AddRange(
    new TaskItem { Title = "Design API" },
    new TaskItem { Title = "Connect Frontend" },
    new TaskItem { Title = "Write Tests" }
  );
  db.SaveChanges();
}

app.Run();
