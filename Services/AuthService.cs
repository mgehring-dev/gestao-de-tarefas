using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
    public async Task<TokenResponseDto?> LoginAsync(UserDto request)
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

      return await CreateTokenResponse(user);
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

    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
      var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
      if (user is null)
      {
        return null;
      }

      return await CreateTokenResponse(user);
    }

    private async Task<TokenResponseDto> CreateTokenResponse(User user)
    {
      return new TokenResponseDto
      {
        AccessToken = CreateToken(user),
        RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
      };
    }

    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
      var user = await context.Users.FindAsync(userId);
      if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
      {
        return null;
      }

      return user;
    }

    private static string GenerateRefreshToken()
    {
      var randomNumber = new byte[32];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);
      return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
      var refreshToken = GenerateRefreshToken();
      user.RefreshToken = refreshToken;
      user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

      await context.SaveChangesAsync();

      return refreshToken;
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
}