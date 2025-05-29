using Ardalis.Result;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Infrastructure.Services.Identity;

namespace BD.PublicPortal.Infrastructure.Interfaces.Identity;

public interface IUserManagementService
{
  Task<Result> RegisterAsync(RegisterUserDto userDto);
  Task<Result<LoginUserCommandResultDTO>> AuthenticateAsync(string email, string password);
}
