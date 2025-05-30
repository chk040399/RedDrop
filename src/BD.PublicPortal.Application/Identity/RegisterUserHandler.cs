using Ardalis.Result;
using BD.SharedKernel;
using System.Threading;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;
using BD.PublicPortal.Infrastructure.Interfaces.Identity;


namespace BD.PublicPortal.Application.Identity;

public class RegisterUserHandler(IUserManagementService userService,IRepository<ApplicationUser> _usersRepo) : IQueryHandler<RegisterUserCommand, Result>
{

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      var count = await  _usersRepo.CountAsync(new ApplicationUserSpecification(request.Dto.DonorNIN));
      if (count > 0)
      {
        return Result.Invalid(new ValidationError("A user with this NIN already exists.")); 
      }

      return await userService.RegisterAsync(request.Dto);
      
    }
}
