namespace GestaoDeTarefas.Module.Tasks.Domain.Models;

public class GetTaskDto
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public DateTime DueDate { get; set; } 
} 