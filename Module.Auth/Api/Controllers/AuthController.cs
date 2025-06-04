using GestaoDeTarefas.Module.Auth.Domain.Models;
using GestaoDeTarefas.Module.Auth.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoDeTarefas.Module.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{

  [HttpPost("login")]
  [SwaggerOperation(Summary = "Login de usuário, retornando um token JWT.")]
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
  [ApiExplorerSettings(IgnoreApi = true)]
  public IActionResult OnlyAuthenticated()
  {
    return Ok("You are authenticated!");
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin-only")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public IActionResult AdminOnly()
  {
    return Ok("You are admin!");
  }
}