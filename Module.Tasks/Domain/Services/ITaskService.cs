using GestaoDeTarefas.Module.Tasks.Domain.Models;
using TaskEntity = GestaoDeTarefas.Module.Tasks.Domain.Entities.Task;

namespace GestaoDeTarefas.Module.Tasks.Domain.Services;

public interface ITaskService
{
  Task<TaskEntity?> RegisterAsync(TaskDto request);
  Task<GetTaskWithUserDto?> GetTaskByIdAsync(int id);
  Task<IEnumerable<GetTaskDto>?> GetAllByIdUserAsync(int id);
  Task<bool?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
  Task<bool> DeleteTaskAsync(int id);
}