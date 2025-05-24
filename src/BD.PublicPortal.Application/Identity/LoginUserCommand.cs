namespace BD.PublicPortal.Application.Identity;
using BD.SharedKernel;



public record LoginUserCommand(string UserEmail, string Password) : IQuery<Result<string>>;
