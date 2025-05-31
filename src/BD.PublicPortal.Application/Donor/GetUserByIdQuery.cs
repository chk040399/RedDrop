using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Application.Identity;

public record GetUserByIdQuery(Guid UserId, int? Level = null) : IQuery<Result<ApplicationUserDTO>>;
