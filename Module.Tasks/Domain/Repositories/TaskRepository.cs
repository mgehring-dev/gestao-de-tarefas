using GestaoDeTarefas.Infra;
using GestaoDeTarefas.Infra.Repositories;
using GestaoDeTarefas.Infra.Repositories.Persistence;
using TaskEntity = GestaoDeTarefas.Module.Tasks.Domain.Entities.Task;

namespace GestaoDeTarefas.Module.Tasks.Domain.Repositories;

public class TaskRepository : RepositoryBase<TaskEntity>, ITaskRepository
{
  public TaskRepository(AppDbContext context) : base(context)
  {
  }
}
