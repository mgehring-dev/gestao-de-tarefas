using GestaoDeTarefas.Module.Tasks.Domain.Models;
using GestaoDeTarefas.Module.Tasks.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoDeTarefas.Module.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TasksController(ITaskService taskService) : ControllerBase
{
  [Authorize]
  [HttpPost]
  [SwaggerOperation(Summary = "Criar uma nova tarefa.")]
  public async Task<ActionResult<string>> CreateTask(TaskDto request)
  {
    var task = await taskService.RegisterAsync(request);
    if (task is null)
    {
      return BadRequest("Task creation failed.");
    }

    return Ok(task.Id);
  }

  [Authorize]
  [HttpGet("{id}")]
  [SwaggerOperation(Summary = "Obter detalhes de uma tarefa.")]
  public async Task<ActionResult<GetTaskWithUserDto>> GetById(int id)
  {
    var task = await taskService.GetTaskByIdAsync(id);
    if (task is null)
    {
      return NotFound("Task not found.");
    }

    return Ok(task);
  }

  [Authorize]
  [HttpGet]
  [SwaggerOperation(Summary = "Listar todas as tarefas atribuídas a um usuário.")]
  public async Task<ActionResult<IEnumerable<GetTaskDto>>> GetAllByIdUser([FromQuery] int id)
  {
    var tasks = await taskService.GetAllByIdUserAsync(id);
    if (tasks is null)
    {
      return NotFound("No tasks found for the specified user.");
    }

    return Ok(tasks);
  }

  [Authorize]
  [HttpPut("{id}")]
  [SwaggerOperation(Summary = "Atualizar informações da tarefa.")]
  public async Task<ActionResult<bool>> Update(int id, UpdateTaskDto request)
  {
    if (id != request.Id)
    {
      return BadRequest("User ID mismatch.");
    }

    var updateTask = await taskService.UpdateTaskAsync(id, request);
    if (updateTask is null)
    {
      return NotFound("Task not found or already deleted.");
    }
    return Ok(updateTask);
  }

  [Authorize]
  [HttpDelete("{id}")]
  [SwaggerOperation(Summary = "Remover uma tarefa.")]
  public async Task<ActionResult<bool>> Delete(int id)
  {
    var result = await taskService.DeleteTaskAsync(id);

    if (!result)
    {
      return NotFound("Task not found or already deleted.");
    }

    return Ok(true);
  }
}