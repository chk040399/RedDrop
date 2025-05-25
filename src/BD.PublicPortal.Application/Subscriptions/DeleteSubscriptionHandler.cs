using BD.PublicPortal.Api.Features.Subscriptions;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Application.Subscriptions;

public class DeleteSubscriptionHandler(IRepository<DonorBloodTransferCenterSubscriptions> _subscriptionRepository)
  : ICommandHandler<DeleteSubscriptionCommand, Result>
{
  public async Task<Result> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
  {
    var existingSubscription = await _subscriptionRepository.GetByIdAsync(request.SubscriptionId, cancellationToken);

    if (existingSubscription == null)
    {
      return Result.NotFound();
    }

    if (existingSubscription.ApplicationUserId != request.ApplicationUserId)
    {
      return Result.Forbidden();
    }

    await _subscriptionRepository.DeleteAsync(existingSubscription, cancellationToken);

    return Result.Success();
  }
}
