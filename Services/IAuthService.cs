using GestaoDeTarefas.Entities;
using GestaoDeTarefas.Models;

namespace GestaoDeTarefas.Services
{
  public interface IAuthService
  {
    Task<User?> RegisterAsync(UserDto request);
    Task<string?> LoginAsync(UserDto request);
  }
}