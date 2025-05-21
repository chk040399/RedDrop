
using BD.SharedKernel;

namespace BD.PublicPortal.Application.Identity.Register;

public record RegisterUserCommand(RegisterUserDto Dto) : IQuery<Result<Guid>>;
