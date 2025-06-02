namespace GestaoDeTarefas.Module.Users;

public interface IUserService
{
    Task<User?> RegisterAsync(UserDto userDto);
    // Task<User?> LoginAsync(UserDto userDto);
    // Task<User?> GetUserByIdAsync(int id);
    // Task<IEnumerable<User>> GetAllUsersAsync();
    // Task<bool> UpdateUserAsync(int id, UserDto userDto);
    // Task<bool> DeleteUserAsync(int id);
}