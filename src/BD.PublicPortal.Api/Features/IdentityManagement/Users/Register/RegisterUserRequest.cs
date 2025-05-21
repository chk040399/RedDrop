
namespace BD.PublicPortal.Api.Features.IdentityManagement.Users.Register;

public class RegisterUserRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }

    public RegisterUserDto ToRegisterUserDto() => new()
    {
      Email = Email,
      Password = Password,
      UserName = UserName,
      PhoneNumber = PhoneNumber
    };
}
