using GestaoDeTarefas.Module.Task;
using GestaoDeTarefas.Module.Users;

namespace GestaoDeTarefas.Infra;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    ITaskRepository Task { get; }
}

