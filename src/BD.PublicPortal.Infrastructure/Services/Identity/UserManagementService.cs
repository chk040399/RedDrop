using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.Result;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Infrastructure.Extensions;
using BD.PublicPortal.Infrastructure.Interfaces.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

#nullable disable

namespace BD.PublicPortal.Infrastructure.Services.Identity;

public class UserManagementService(UserManager<ApplicationUser> userManager, IConfiguration configuration) : IUserManagementService
{
  public async Task<Result> RegisterAsync(RegisterUserDto userDto)
  {
    var user = userDto.ToEntityEx();
    var result = await userManager.CreateAsync(user, userDto.Password);
    return result.ToSmartResult();
  }

  public async Task<Result<string>> AuthenticateAsync(string email, string password)
  {
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
      return Result<string>.NotFound();

    if (!await userManager.CheckPasswordAsync(user, password))
    {
      return Result<string>.Invalid(new []{new ValidationError("Incorrect Password")});
    }

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[]
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim("UserId", user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email!)
      }),
      Expires = DateTime.UtcNow.AddDays(1),
      Issuer = configuration["JwtSettings:Issuer"],
      Audience = configuration["JwtSettings:Audience"],
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return Result<string>.Success(tokenHandler.WriteToken(token));
  }
}
