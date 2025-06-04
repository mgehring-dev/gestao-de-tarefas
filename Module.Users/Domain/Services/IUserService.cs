using GestaoDeTarefas.Module.Users.Domain.Entities;
using GestaoDeTarefas.Module.Users.Domain.Models;

namespace GestaoDeTarefas.Module.Users.Domain.Services;

public interface IUserService
{
    Task<User?> RegisterAsync(UserDto userDto);
    Task<GetUserDto?> GetUserByIdAsync(int id);
    Task<bool?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(int id);
}