using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestaoDeTarefas.Entities;
using GestaoDeTarefas.Infra.Persistence;
using GestaoDeTarefas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GestaoDeTarefas.Services
{

  public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
  {
    public async Task<string?> LoginAsync(UserDto request)
    {
      var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
      if (user is null)
      {
        return null;
      }

      var passwordResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);

      if (passwordResult == PasswordVerificationResult.Failed)
      {
        return null;
      }

      return CreateToken(user);
    }

    public async Task<User?> RegisterAsync(UserDto request)
    {
      if (await context.Users.AnyAsync(u => u.UserName == request.UserName))
      {
        return null;
      }

      var user = new User();
      var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

      user.UserName = request.UserName;
      user.PasswordHash = hashedPassword;

      context.Users.Add(user);
      await context.SaveChangesAsync();

      return user;
    }

    private string CreateToken(User user)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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