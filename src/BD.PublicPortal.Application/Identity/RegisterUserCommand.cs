using BD.PublicPortal.Infrastructure.Services.Identity;
using BD.SharedKernel;

namespace BD.PublicPortal.Application.Identity;

public record RegisterUserCommand(RegisterUserDto Dto) : IQuery<Result>;
