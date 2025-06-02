
using GestaoDeTarefas.Infra;

namespace GestaoDeTarefas.Module.Users;

public class UserService : IUserService
{
  private readonly IUnitOfWork _unitOfWork;

  public UserService(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public async Task<User?> RegisterAsync(UserDto userDto)
  {
    var exists = await _unitOfWork.User.ObterComCondicaoAsync(u => u.UserName == userDto.UserName);
    if (exists.Any())
    {
      return null;
    }

    var user = new User
    {
      UserName = userDto.UserName,
    };

    return user;
  }
}

