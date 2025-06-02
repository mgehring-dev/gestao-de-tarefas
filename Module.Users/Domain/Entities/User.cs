using GestaoDeTarefas.Infra;

namespace GestaoDeTarefas.Module.Users;

public class User : EntityBase
{
  public string UserName { get; set; } = string.Empty;
  public string PasswordHash { get; set; } = string.Empty;
  public string Role { get; set; } = string.Empty;
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiryTime { get; set; }

}
