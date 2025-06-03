namespace GestaoDeTarefas.Module.Auth;

public interface IAuthService
{
  Task<string?> LoginAsync(LoginDto request);
}
