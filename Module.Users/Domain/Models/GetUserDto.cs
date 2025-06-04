namespace GestaoDeTarefas.Module.Users.Domain.Models;

public class GetUserDto
{
  public string UserName { get; set; } = string.Empty;
  public string Role { get; set; } = string.Empty;
}
