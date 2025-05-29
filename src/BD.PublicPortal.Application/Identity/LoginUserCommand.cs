

using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Application.Identity;
using BD.SharedKernel;


public record LoginUserCommand(string UserEmail, string Password) : IQuery<Result<LoginUserCommandResultDTO>>;
