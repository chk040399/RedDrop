using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Pledges;

public class ListPledgesHandler(IReadRepository<BloodDonationPledge> _pledgeRepo) : IQueryHandler<ListPledgesQuery, Result<IEnumerable<BloodDonationPledgeDTO>>>
{
  public async Task<Result<IEnumerable<BloodDonationPledgeDTO>>> Handle(ListPledgesQuery request, CancellationToken cancellationToken)
  {
    BloodDonationPledgeSpecification spec = new BloodDonationPledgeSpecification(filter: request.filter, loggedUserId: request.LoggedUserID, level: request.Level);
    var lst = await _pledgeRepo.ListAsync(spec, cancellationToken);
    var level = (request.Level == null) ? 0 : (int)request.Level;
    return Result<IEnumerable<BloodDonationPledgeDTO>>.Success(lst.ToDtosWithRelated(level));
  }
}
