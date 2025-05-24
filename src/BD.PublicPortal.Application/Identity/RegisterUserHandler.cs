using Ardalis.Result;
using BD.SharedKernel;
using System.Threading;
using BD.PublicPortal.Infrastructure.Interfaces.Identity;


namespace BD.PublicPortal.Application.Identity;

public class RegisterUserHandler(IUserManagementService userService) : IQueryHandler<RegisterUserCommand, Result>
{

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      return await userService.RegisterAsync(request.Dto);
      
    }
}
