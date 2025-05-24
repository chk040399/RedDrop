using BD.PublicPortal.Infrastructure.Interfaces.Identity;

namespace BD.PublicPortal.Application.Identity;



public class LoginUserHandler(IUserManagementService userService) : IQueryHandler<LoginUserCommand, Result<string>>
{ 

  public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
  {
    return await userService.AuthenticateAsync(request.UserEmail,request.Password);

  }
}
