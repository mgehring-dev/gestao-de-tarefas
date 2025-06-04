using GestaoDeTarefas.Infra.Repositories;
using GestaoDeTarefas.Infra.Repositories.Persistence;
using GestaoDeTarefas.Module.Users.Domain.Entities;

namespace GestaoDeTarefas.Module.Users.Domain.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
  public UserRepository(AppDbContext context) : base(context)
  {
  }
}
