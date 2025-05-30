using GestaoDeTarefas.Entities;
using GestaoDeTarefas.Models;
using GestaoDeTarefas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeTarefas.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController(IAuthService authService) : ControllerBase
  {

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
      var user = await authService.RegisterAsync(request);
      if (user is null)
      {
        return BadRequest("Username already exists.");
      }

      return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
    {
      var token = await authService.LoginAsync(request);
      if (token is null)
      {
        return BadRequest("Invalid username or password.");
      }

      return Ok(token);
    }

    [HttpPost("refresh-tokens")]
    public async Task<ActionResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request)
    {
      var result = await authService.RefreshTokensAsync(request);
      if (result is null || result.AccessToken is null || result.RefreshToken is null)
      {
        return Unauthorized("Invalid refresh token.");
      } 
      
      return Ok(result);
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
}