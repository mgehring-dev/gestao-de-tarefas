using GestaoDeTarefas.Module.Users;

namespace GestaoDeTarefas.Infra;

public interface IUnitOfWork
{
    IUserRepository User { get; }
}

