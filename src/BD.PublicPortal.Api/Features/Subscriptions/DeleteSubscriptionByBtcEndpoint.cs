using BD.PublicPortal.Application.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace BD.PublicPortal.Api.Features.Subscriptions;
  

public class DeleteSubscriptionByBtcRequest
{
  [FromClaim(claimType: "UserId", isRequired: true)]
  public Guid ApplicationUserId { get; set; }

  [FromRoute]
  public Guid BtcId { get; set; }
}

public class DeleteSubscriptionByBtcEndpoint : Endpoint<DeleteSubscriptionByBtcRequest>
{
  private readonly IMediator _mediator;

  public DeleteSubscriptionByBtcEndpoint(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Delete("/Subscriptions/btc/{BtcId}");
  }

  public override async Task HandleAsync(DeleteSubscriptionByBtcRequest req, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new DeleteSubscriptionByBtcCommand(
      ApplicationUserId: req.ApplicationUserId,
      BtcId: req.BtcId), cancellationToken);

    if (result.IsSuccess)
      await SendOkAsync(cancellationToken);
  }
}
