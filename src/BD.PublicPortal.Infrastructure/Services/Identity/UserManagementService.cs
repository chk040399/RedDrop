using Ardalis.Result;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Infrastructure.Extensions;
using BD.PublicPortal.Infrastructure.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;

namespace BD.PublicPortal.Infrastructure.Services.Identity;


public class UserManagementService(UserManager<ApplicationUser> userManager) : IUserManagementService
{
  public async Task<Result<Guid>> RegisterUserAsync(RegisterUserDto dto)
    {
        var user = dto.ToEntityEx();

        var result = await userManager.CreateAsync(user, dto.Password);

        return result.ToSmartResult(user.Id);
    }
}
