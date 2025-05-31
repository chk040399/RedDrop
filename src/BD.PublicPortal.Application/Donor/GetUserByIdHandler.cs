using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Identity;

public class GetUserByIdHandler(IReadRepository<ApplicationUser> _userRepo) : IQueryHandler<GetUserByIdQuery, Result<ApplicationUserDTO>>
{
  public async Task<Result<ApplicationUserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
  {
    var level = (request.Level == null) ? 0 : (int)request.Level;
    var user = await _userRepo.FirstOrDefaultAsync(new ApplicationUserSpecificationByLevel(request.UserId,level: request.Level), cancellationToken);
    
    if (user == null)
    {
      return Result<ApplicationUserDTO>.NotFound();
    }


    var result = user.ToDtoWithRelated(level);
    return Result<ApplicationUserDTO>.Success(result);
  }
}
