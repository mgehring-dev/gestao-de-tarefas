using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeTarefas.Infra.Entities;

[ExcludeFromCodeCoverage]
public class EntityBase
{
  [Key]
  public virtual int Id { get; set; }
  public DateTime CriadoEm { get; set; }
  public DateTime? AtualizadoEm { get; set; }
}
