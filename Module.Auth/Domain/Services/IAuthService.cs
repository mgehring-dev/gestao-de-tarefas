using GestaoDeTarefas.Module.Auth.Domain.Models;

namespace GestaoDeTarefas.Module.Auth.Domain.Services;

public interface IAuthService
{
  Task<string?> LoginAsync(LoginDto request);
}
