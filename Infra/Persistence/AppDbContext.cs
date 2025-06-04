using GestaoDeTarefas.Module.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TaskEntity = GestaoDeTarefas.Module.Tasks.Domain.Entities.Task;

namespace GestaoDeTarefas.Infra.Repositories.Persistence;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }
  public DbSet<User> Users { get; set; }
  public DbSet<TaskEntity> Tasks { get; set; }
}
