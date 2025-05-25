namespace BD.PublicPortal.Api.Features.Subscriptions;

public record DeleteSubscriptionCommand(
  Guid SubscriptionId,
  Guid ApplicationUserId
) : ICommand<Result>;
