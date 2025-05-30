namespace BD.PublicPortal.Application.Subscriptions;

public record DeleteSubscriptionByBtcCommand(
  Guid ApplicationUserId,
  Guid BtcId
) : ICommand<Result>;
