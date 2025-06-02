using GestaoDeTarefas.Infra;

namespace GestaoDeTarefas.Module.Users;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
  public UserRepository(AppDbContext context) : base(context)
  {
  }
}
