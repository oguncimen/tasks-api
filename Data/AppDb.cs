using Microsoft.EntityFrameworkCore;
using TasksApi.Models;

namespace TasksApi.Data {
  public class AppDb : DbContext {
    public AppDb(DbContextOptions<AppDb> opts) : base(opts) { }
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
  }
}
