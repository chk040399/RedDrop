using BD.PublicPortal.Application.Subscriptions;
using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Api.Features.Subscriptions;

public class CreateSubscriptionRequest
{
  public Guid BloodTansfusionCenterId { get; set; }
  public Guid ApplicationUserId { get; set; }

  //[FromClaim(claimType: "UserId", isRequired: true)]
  //public Guid LoggedUserId { get; set; }
}

public class CreateSubscriptionResponse
{
  public DonorBloodTransferCenterSubscriptionsDTO Subscription { get; set; } = null!;
}

public class CreateSubscriptionEndpoint(IMediator _mediator) : Endpoint<CreateSubscriptionRequest, CreateSubscriptionResponse>
{
  public override void Configure()
  {
    Post("/subscriptions");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateSubscriptionRequest req, CancellationToken cancellationToken)
  {
    var command = new CreateSubscriptionCommand(
      req.BloodTansfusionCenterId,
      req.ApplicationUserId
    );

    var result = await _mediator.Send(command, cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CreateSubscriptionResponse()
      {
        Subscription = result.Value
      };
    }
  }
}
