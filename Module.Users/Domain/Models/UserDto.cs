namespace GestaoDeTarefas.Module.Users.Domain.Models;

public class UserDto
{
  public string UserName { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string Role { get; set; } = string.Empty;
}
