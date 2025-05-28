using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Application.Donor;

public record UpdateProfileCommand(UpdateUserDTO UpdateUserDTO, Guid UserId) : ICommand<Result<ApplicationUserDTO>>;
