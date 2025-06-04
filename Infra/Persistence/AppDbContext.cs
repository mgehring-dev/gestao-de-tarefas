
using GestaoDeTarefas.Module.Users;
using Microsoft.EntityFrameworkCore;
using TaskEntity = GestaoDeTarefas.Module.Task.Task;

namespace GestaoDeTarefas.Infra;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }
  public DbSet<User> Users { get; set; }
  public DbSet<TaskEntity> Tasks { get; set; }
}
