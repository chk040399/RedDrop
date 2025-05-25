using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.BTCsubscribed;

public class GetBTCsubscribedHandler(IReadRepository<DonorBloodTransferCenterSubscriptions> _btcRepo) : IQueryHandler<GetBTCsubscribedQuery, Result<IEnumerable<DonorBloodTransferCenterSubscriptionsDTO>>>
{
  public async Task<Result<IEnumerable<DonorBloodTransferCenterSubscriptionsDTO>>> Handle(GetBTCsubscribedQuery request, CancellationToken cancellationToken)
  {
    BTCSubscribedSpecification spec = new BTCSubscribedSpecification(filter: request.filter, loggedUserId: request.LoggedUserID, level: request.Level);
    var lst = await _btcRepo.ListAsync(spec, cancellationToken);
    var level = (request.Level == null) ? 0 : (int)request.Level;
    return Result<IEnumerable<DonorBloodTransferCenterSubscriptionsDTO>>.Success(lst.ToDtosWithRelated(level));
  }
}
