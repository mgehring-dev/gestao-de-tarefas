namespace GestaoDeTarefas.Module.Task;

public class GetTaskWithUserDto
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public DateTime DueDate { get; set; }
  public TaskUserDto? User { get; set; }
}

public class TaskUserDto
{
  public int Id { get; set; }
  public string UserName { get; set; } = string.Empty;
}