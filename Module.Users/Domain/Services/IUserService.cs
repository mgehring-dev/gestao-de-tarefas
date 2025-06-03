namespace GestaoDeTarefas.Module.Users;

public interface IUserService
{
    Task<User?> RegisterAsync(UserDto userDto);
    Task<GetUserDto?> GetUserByIdAsync(int id);
    Task<bool?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(int id);
}