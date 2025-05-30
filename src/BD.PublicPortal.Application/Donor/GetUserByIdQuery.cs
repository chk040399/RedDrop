using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Application.Identity;

public record GetUserByIdQuery(Guid UserId) : IQuery<Result<ApplicationUserDTO>>;
