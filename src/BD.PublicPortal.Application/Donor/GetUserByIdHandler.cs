using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Identity;

public class GetUserByIdHandler(IReadRepository<ApplicationUser> _userRepo) : IQueryHandler<GetUserByIdQuery, Result<ApplicationUserDTO>>
{
  public async Task<Result<ApplicationUserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
  { 

    var user = await _userRepo.GetByIdAsync<Guid>(request.UserId, cancellationToken);
    if (user == null)
    {
      return Result<ApplicationUserDTO>.NotFound();
    }


    var result = user.ToDtoWithRelated(1);
    return Result<ApplicationUserDTO>.Success(result);
  }
}
