namespace BD.PublicPortal.Api.Features.Subscriptions;

public class DeleteSubscriptionRequest
{
  [FromClaim(claimType: "UserId", isRequired: true)]
  public Guid ApplicationUserId { get; set; }
  public Guid SubscriptionId { get; set; }
}

public class DeleteSubscriptionEndpoint(IMediator _mediator) : Endpoint<DeleteSubscriptionRequest>
{
  public override void Configure()
  {
    Delete("/subscriptions/{SubscriptionId:guid}");
  }

  public override async Task HandleAsync(DeleteSubscriptionRequest req, CancellationToken cancellationToken)
  {
    var command = new DeleteSubscriptionCommand(req.SubscriptionId, req.ApplicationUserId);

    var result = await _mediator.Send(command, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      await SendOkAsync(cancellationToken);
    }
  }
}
