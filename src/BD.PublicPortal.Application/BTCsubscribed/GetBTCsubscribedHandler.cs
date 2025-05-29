using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.BTCsubscribed;

public class GetBTCsubscribedHandler(
  IReadRepository<DonorBloodTransferCenterSubscriptions> subscriptionsRepo)
  : IQueryHandler<GetBTCsubscribedQuery, Result<IEnumerable<DonorBloodTransferCenterSubscriptionsDTO>>>
{
  public async Task<Result<IEnumerable<DonorBloodTransferCenterSubscriptionsDTO>>> Handle(
    GetBTCsubscribedQuery request,
    CancellationToken cancellationToken)
  {
    var spec = new UserSubscriptionsSpecification(
      userId: request.LoggedUserId,
      paginationTake: request.PaginationTake,
      level: request.Level);

    var subscriptions = await subscriptionsRepo.ListAsync(spec, cancellationToken);
    var level = request.Level ?? 0;

    return Result<IEnumerable<DonorBloodTransferCenterSubscriptionsDTO>>.Success(
      subscriptions.ToDtosWithRelated(level));
  }
}
