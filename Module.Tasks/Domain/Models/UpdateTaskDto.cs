using GestaoDeTarefas.Module.Tasks.Domain.Enums;

namespace GestaoDeTarefas.Module.Tasks.Domain.Models;

public class UpdateTaskDto
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public EnumStatus Status { get; set; } = EnumStatus.New;
  public DateTime DueDate { get; set; } 
  public int IdUser { get; set; }
}
