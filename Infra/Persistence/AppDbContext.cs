
using GestaoDeTarefas.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeTarefas.Infra.Persistence
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
  }
}
