using System.ComponentModel.DataAnnotations.Schema;
using GestaoDeTarefas.Infra;

namespace GestaoDeTarefas.Module.Task;

public class Task : EntityBase
{
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public EnumStatus Status { get; set; } = EnumStatus.New;
  public DateTime DueDate { get; set; }
  [ForeignKey("IdUser")]
  public int IdUser { get; set; } 
}