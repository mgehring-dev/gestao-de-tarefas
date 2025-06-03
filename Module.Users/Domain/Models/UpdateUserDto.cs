namespace GestaoDeTarefas.Module.Users;

public class UpdateUserDto
{
  public int Id { get; set; }
  public string UserName { get; set; } = string.Empty; 
  public string Role { get; set; } = string.Empty;
}
