using GestaoDeTarefas.Infra;
using GestaoDeTarefas.Infra.UnitOfWork;
using GestaoDeTarefas.Module.Tasks.Domain.Enums;
using GestaoDeTarefas.Module.Tasks.Domain.Models;
using TaskEntity = GestaoDeTarefas.Module.Tasks.Domain.Entities.Task;

namespace GestaoDeTarefas.Module.Tasks.Domain.Services;

public class TaskService : ITaskService
{
  private readonly IUnitOfWork _unitOfWork;

  public TaskService(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public async Task<TaskEntity?> RegisterAsync(TaskDto request)
  {
    var user = await _unitOfWork.User.ObterComCondicaoAsync(u => u.Id == request.IdUser);
    if (user == null || !user.Any() || user.First().IsDeleted)
    {
      return null;
    }

    var task = new TaskEntity
    {
      Title = request.Title,
      Description = request.Description,
      Status = request.Status,
      IdUser = request.IdUser,
      DueDate = request.DueDate
    };

    return await _unitOfWork.Task.SalvarAsync(task);
  }

  private static string GetStatusDescription(EnumStatus status)
  {
    return status switch
    {
      EnumStatus.New => "New",
      EnumStatus.Active => "Active",
      EnumStatus.Closed => "Closed",
      _ => throw new NotImplementedException()
    };
  }

  public async Task<GetTaskWithUserDto?> GetTaskByIdAsync(int id)
  {
    var task = await _unitOfWork.Task.FindAsync(id);
    if (task == null)
    {
      return null;
    }

    var user = await _unitOfWork.User.FindAsync(task.IdUser);

    return new GetTaskWithUserDto
    {
      Id = task.Id,
      Title = task.Title,
      Description = task.Description,
      DueDate = task.DueDate,
      Status = GetStatusDescription(task.Status),
      User = user == null
        ? null
        : new TaskUserDto
        {
          Id = user.Id,
          UserName = user.UserName
        }
    };
  }

  public async Task<IEnumerable<GetTaskDto>?> GetAllByIdUserAsync(int id)
  {
    var tasks = await _unitOfWork.Task.ObterComCondicaoAsync(t => t.IdUser == id);
    if (tasks == null || !tasks.Any())
    {
      return Enumerable.Empty<GetTaskDto>();
    }

    return tasks.Select(task => new GetTaskDto
    {
      Id = task.Id,
      Title = task.Title,
      Description = task.Description,
      DueDate = task.DueDate,
      Status = GetStatusDescription(task.Status)
    });
  }

  public async Task<bool?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
  {
    var user = await _unitOfWork.User.FindAsync(updateTaskDto.IdUser);
    if (user == null || user.IsDeleted)
    {
      return null;
    }

    var task = await _unitOfWork.Task.FindAsync(updateTaskDto.Id);
    if (task == null)
    {
      return null;
    }

    task.Title = updateTaskDto.Title;
    task.Description = updateTaskDto.Description;
    task.Status = updateTaskDto.Status;
    task.IdUser = updateTaskDto.IdUser;
    task.AtualizadoEm = DateTime.Now;

    return await _unitOfWork.Task.SalvarAsync(task) != null;
  }

  public async Task<bool> DeleteTaskAsync(int id)
  {
    var task = await _unitOfWork.Task.FindAsync(id);
    if (task == null)
    {
      return false;
    }

    await _unitOfWork.Task.DeletarFisicamenteAsync(task);
    return true;
  }
}