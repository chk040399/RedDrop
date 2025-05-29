using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Infrastructure.Interfaces.Identity;

namespace BD.PublicPortal.Application.Identity;



public class LoginUserHandler(IUserManagementService userService) : IQueryHandler<LoginUserCommand, Result<LoginUserCommandResultDTO>>
{ 

  public async Task<Result<LoginUserCommandResultDTO>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
  {
    return await userService.AuthenticateAsync(request.UserEmail,request.Password);

  }
}
