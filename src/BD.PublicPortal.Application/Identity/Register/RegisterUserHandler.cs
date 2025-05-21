using Ardalis.Result;
using BD.PublicPortal.Core.Interfaces.Identity;
using System.Threading;


namespace BD.PublicPortal.Application.Identity.Register;

public class RegisterUserHandler : IQueryHandler<RegisterUserCommand, Result<string>>
{
    private readonly IUserManagementService _userService;

    public RegisterUserHandler(IUserManagementService userService)
    {
        _userService = userService;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      return await _userService.RegisterUserAsync(request.Dto);
      
    }
}
