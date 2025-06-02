
using GestaoDeTarefas.Module.Users;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeTarefas.Infra;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }
  public DbSet<User> Users { get; set; }
}
