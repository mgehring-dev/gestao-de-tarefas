using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestaoDeTarefas.Entities;
using GestaoDeTarefas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GestaoDeTarefas.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController(IConfiguration configuration) : ControllerBase
  {
    public static User user = new();

    [HttpPost("register")]
    public ActionResult<User> Register(UserDto request)
    {
      var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

      user.UserName = request.UserName;
      user.PasswordHash = hashedPassword;

      return Ok(user);
    }

    [HttpPost("login")]
    public ActionResult<string> Login(UserDto request)
    {
      if (user.UserName != request.UserName)
      {
        return BadRequest("User not found");
      }

      var passwordResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);

      if (passwordResult == PasswordVerificationResult.Failed)
      {
        return BadRequest("Wrong password");
      }
      else
      {
        string token = CreateToken(user);

        return Ok(token);
      }
    }

    private string CreateToken(User user)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, user.UserName)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

      var tokenDescriptor = new JwtSecurityToken(
        issuer: configuration.GetValue<string>("AppSettings:Issuer"),
        audience: configuration.GetValue<string>("AppSettings:Audience"),
        claims: claims,
        expires: DateTime.UtcNow.AddDays(1),
        signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
  }
}