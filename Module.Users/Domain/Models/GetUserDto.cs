using GestaoDeTarefas.Infra;

namespace GestaoDeTarefas.Module.Users;

public class GetUserDto
{
  public string UserName { get; set; } = string.Empty; 
  public string Role { get; set; } = string.Empty;
}
