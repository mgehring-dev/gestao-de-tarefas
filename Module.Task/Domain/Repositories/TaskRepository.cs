using GestaoDeTarefas.Infra;

namespace GestaoDeTarefas.Module.Task;

public class TaskRepository : RepositoryBase<Task>, ITaskRepository
{
  public TaskRepository(AppDbContext context) : base(context)
  {
  }
}
