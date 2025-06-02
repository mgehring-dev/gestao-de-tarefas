using Microsoft.AspNetCore.Mvc;

namespace GestaoDeTarefas.Module.Users;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  [HttpPost]
  public async Task<ActionResult<User>> Register([FromServices] IUserService userService, UserDto request)
  {
    var user = await userService.RegisterAsync(request);
    if (user is null)
    {
      return BadRequest("Username already exists.");
    }

    return Ok(user);
  }
}