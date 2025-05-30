using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Identity.List;

public class ListApplicationUsersHandler(IReadRepository<ApplicationUser> _userRepo) : IQueryHandler<ListApplicationUsersQuery, Result<IEnumerable<ApplicationUserDTO>>>
{
  public async Task<Result<IEnumerable<ApplicationUserDTO>>> Handle(ListApplicationUsersQuery request, CancellationToken cancellationToken)
  {
    ApplicationUserSpecification spec = new ApplicationUserSpecification(filter: request.filter, loggedUserId: request.LoggedUserID, level: request.Level);
    var lst = await _userRepo.ListAsync(spec, cancellationToken);
    var level = (request.Level == null) ? 0 : (int)request.Level;
    return Result<IEnumerable<ApplicationUserDTO>>.Success(lst.ToDtosWithRelated(level));
  }
}
