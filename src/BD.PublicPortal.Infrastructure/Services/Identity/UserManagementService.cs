using Ardalis.Result;
using BD.PublicPortal.Core.Entities;
using Microsoft.AspNetCore.Identity;
using BD.PublicPortal.Core.Interfaces.Identity;

namespace BD.PublicPortal.Infrastructure.Services.Identity;

public class UserManagementService(UserManager<ApplicationUser> userManager) : IUserManagementService
{
  public async Task<Result<Guid>> RegisterUserAsync(RegisterUserDto dto)
    {
        var user = dto.ToApplicationUser();

        var result = await userManager.CreateAsync(user, dto.Password);

        return result.ToSmartResult(user.Id);
    }
}
