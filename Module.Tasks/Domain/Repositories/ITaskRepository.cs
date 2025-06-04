using GestaoDeTarefas.Infra.Repositories;
using TaskEntity = GestaoDeTarefas.Module.Tasks.Domain.Entities.Task;

namespace GestaoDeTarefas.Module.Tasks.Domain.Repositories;

public interface ITaskRepository : IRepositoryBase<TaskEntity>
{
}