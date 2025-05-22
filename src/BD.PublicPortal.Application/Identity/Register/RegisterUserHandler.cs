using Ardalis.Result;
using BD.SharedKernel;
using System.Threading;
using BD.PublicPortal.Infrastructure.Interfaces.Identity;


namespace BD.PublicPortal.Application.Identity.Register;

public class RegisterUserHandler : IQueryHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IUserManagementService _userService;

    public RegisterUserHandler(IUserManagementService userService)
    {
        _userService = userService;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      return await _userService.RegisterUserAsync(request.Dto);
      
    }
}
