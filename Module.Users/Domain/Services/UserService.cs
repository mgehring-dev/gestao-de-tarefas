
using GestaoDeTarefas.Infra;
using Microsoft.AspNetCore.Identity;

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

    var user = new User();
    var hashedPassword = new PasswordHasher<User>().HashPassword(user, userDto.Password);

    user.UserName = userDto.UserName;
    user.PasswordHash = hashedPassword;
    user.Role = userDto.Role;

    return await _unitOfWork.User.SalvarAsync(user);
  }

  public async Task<GetUserDto?> GetUserByIdAsync(int id)
  {
    var user = await _unitOfWork.User.FindAsync(id);
    if (user == null || user.IsDeleted)
    {
      return null;
    }
    return new GetUserDto
    {
      UserName = user.UserName,
      Role = user.Role,
    };
  }

  public async Task<bool?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
  {
    var user = await _unitOfWork.User.FindAsync(id);
    if (user == null || user.IsDeleted)
    {
      return null;
    }

    user.UserName = updateUserDto.UserName;
    user.Role = updateUserDto.Role;
    user.AtualizadoEm = DateTime.Now;

    return await _unitOfWork.User.SalvarAsync(user) != null;
  }

  public async Task<bool> DeleteUserAsync(int id)
  {
    var user = await _unitOfWork.User.FindAsync(id);
    if (user == null)
    {
      return false;
    }

    user.IsDeleted = true;
    user.DeletedAt = DateTime.Now;
    user.AtualizadoEm = DateTime.Now;

    await _unitOfWork.User.SalvarAsync(user);
    return true;
  }
}

