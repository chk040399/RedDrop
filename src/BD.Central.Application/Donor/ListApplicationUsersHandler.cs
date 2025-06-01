using BD.Central.Core.DTOs;
using BD.Central.Core.Entities;
using BD.Central.Core.Entities.Specifications;


namespace BD.Central.Application.Identity.List;

public class ListApplicationUsersHandler(IReadRepository<ApplicationUser> _userRepo) : IQueryHandler<ListApplicationUsersQuery, Result<IEnumerable<ApplicationUserDTO>>>
{
  public async Task<Result<IEnumerable<ApplicationUserDTO>>> Handle(ListApplicationUsersQuery request, CancellationToken cancellationToken)
  {
    ApplicationUserSpecification spec = new ApplicationUserSpecification(filter: request.filter, level: request.Level);
    var lst = await _userRepo.ListAsync(spec, cancellationToken);
    var level = (request.Level == null) ? 0 : (int)request.Level;
    return Result<IEnumerable<ApplicationUserDTO>>.Success(lst.ToDtosWithRelated(level));
  }
}
