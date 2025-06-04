using TaskEntity = GestaoDeTarefas.Module.Task.Task;

namespace GestaoDeTarefas.Module.Task;

public interface ITaskService
{
  Task<TaskEntity?> RegisterAsync(TaskDto request);
  Task<GetTaskWithUserDto?> GetTaskByIdAsync(int id);
  Task<IEnumerable<GetTaskDto>?> GetAllByIdUserAsync(int id);
  Task<bool?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
  Task<bool> DeleteTaskAsync(int id);
}