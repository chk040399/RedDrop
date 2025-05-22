using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Infrastructure.Services.Identity;

public class RegisterUserDto: ApplicationUserDTO
{
  public string UserName { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;

  public string Password { get; set; } = string.Empty;
  public string ConfirmPassword { get; set; } = string.Empty;

  public ApplicationUser ToEntityEx()
  {
    var user = this.ToEntity();
    user.UserName = UserName;
    user.Email = Email;
    return user;
  }


}
