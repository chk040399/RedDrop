using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Subscriptions;

public class DeleteSubscriptionByBtcHandler(
  IRepository<DonorBloodTransferCenterSubscriptions> subscriptionsRepo)
  : ICommandHandler<DeleteSubscriptionByBtcCommand, Result>
{
  public async Task<Result> Handle(DeleteSubscriptionByBtcCommand request, CancellationToken cancellationToken)
  {
    var spec = new ExistingSubscriptionSpecification(request.ApplicationUserId, request.BtcId);
    var subscription = await subscriptionsRepo.FirstOrDefaultAsync(spec, cancellationToken);

    if (subscription == null)
      return Result.NotFound("Subscription not found");

    await subscriptionsRepo.DeleteAsync(subscription, cancellationToken);
    return Result.Success();
  }
}
