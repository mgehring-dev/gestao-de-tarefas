using GestaoDeTarefas.Infra.Repositories;
using GestaoDeTarefas.Module.Users.Domain.Entities;

namespace GestaoDeTarefas.Module.Users.Domain.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
}