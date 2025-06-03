using Microsoft.AspNetCore.Mvc;

namespace GestaoDeTarefas.Module.Users;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
  [HttpPost]
  public async Task<ActionResult<string>> Register(UserDto request)
  {
    var user = await userService.RegisterAsync(request);
    if (user is null)
    {
      return BadRequest("Username already exists.");
    }

    return Ok(user.Id);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<GetUserDto>> GetById(int id)
  {
    var user = await userService.GetUserByIdAsync(id);
    if (user is null)
    {
      return NotFound("User not found.");
    }

    return Ok(user);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<GetUserDto>> Update(int id, UpdateUserDto request)
  {
    if (id != request.Id)
    {
      return BadRequest("User ID mismatch.");
    }

    var updateUser = await userService.UpdateUserAsync(id, request);
    if (updateUser is null)
    {
      return NotFound("User not found or already deleted.");
    }
    return Ok(updateUser);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<bool>> Delete(int id)
  {
    var result = await userService.DeleteUserAsync(id);

    if (!result)
    {
      return NotFound("User not found.");
    }

    return Ok(true);
  }
}