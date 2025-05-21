using Ardalis.Result;

namespace BD.PublicPortal.Core.Interfaces.Identity;

public interface IUserManagementService
{
    Task<Result<Guid>> RegisterUserAsync(RegisterUserDto dto);
}
