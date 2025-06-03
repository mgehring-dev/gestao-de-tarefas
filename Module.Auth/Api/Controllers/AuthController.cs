using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeTarefas.Module.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{

  [HttpPost("login")]
  public async Task<ActionResult<string>> Login(LoginDto request)
  {
    var token = await authService.LoginAsync(request);
    if (token is null)
    {
      return BadRequest("Invalid username or password.");
    }

    return Ok(token);
  } 

  [Authorize]
  [HttpGet]
  public IActionResult OnlyAuthenticated()
  {
    return Ok("You are authenticated!");
  }
  
  [Authorize(Roles = "Admin")]
  [HttpGet("admin-only")]
  public IActionResult AdminOnly()
  {
    return Ok("You are admin!");
  }
}