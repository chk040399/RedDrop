using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Identity;

public class GetUserByIdHandler(IReadRepository<ApplicationUser> _userRepo) : IQueryHandler<GetUserByIdQuery, Result<ApplicationUserDTO>>
{
  public async Task<Result<ApplicationUserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
  {

    if (request.UserId == null)
    {
      Result<ApplicationUserDTO?>.NotFound();
    }


    var spec = new ApplicationUserSpecification(request.UserId);
    var users = await _userRepo.ListAsync(spec, cancellationToken);
    var user = users.FirstOrDefault();
    if (user == null)
    {
      return Result<ApplicationUserDTO>.NotFound();
    }


    var result = user.ToDtoWithRelated(1);
    return Result<ApplicationUserDTO>.Success(result);
  }
}
