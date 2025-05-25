using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Application.Subscriptions;

public record CreateSubscriptionCommand(
  Guid BloodTansfusionCenterId,
  Guid ApplicationUserId
) : ICommand<Result<DonorBloodTransferCenterSubscriptionsDTO>>;

