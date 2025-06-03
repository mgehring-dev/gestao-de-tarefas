using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestaoDeTarefas.Infra;
using GestaoDeTarefas.Module.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GestaoDeTarefas.Module.Auth;

public class AuthService(IUnitOfWork unitOfWork, IConfiguration configuration) : IAuthService
{
  public async Task<string?> LoginAsync(LoginDto request)
  {
    var user = (await unitOfWork.User.ObterComCondicaoAsync(u => u.UserName == request.UserName)).FirstOrDefault();
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

  private string CreateToken(User user)
  {
    var claims = new List<Claim>
        {
          new Claim(ClaimTypes.Name, user.UserName),
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Role, user.Role)
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
