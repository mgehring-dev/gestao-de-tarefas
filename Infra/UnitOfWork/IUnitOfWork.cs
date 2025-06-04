 using GestaoDeTarefas.Module.Tasks.Domain.Repositories;
using GestaoDeTarefas.Module.Users.Domain.Repositories;

namespace GestaoDeTarefas.Infra.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    ITaskRepository Task { get; }
}

