namespace GestaoDeTarefas.Module.Task;

public class TaskDto
{
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public int IdUser { get; set; }
  public DateTime DueDate { get; set; } = DateTime.Now.AddHours(1);
  public EnumStatus Status { get; set; } = EnumStatus.New;
}