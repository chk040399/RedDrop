using BD.PublicPortal.Application.BTCsubscribed;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Api.Features.BTC;

public class GetBTCsubscribedRequest
{
  [FromClaim(claimType: "UserId", isRequired: true)]
  public Guid LoggedUserId { get; set; }
  public int? PaginationTake { get; set; } = null;
  public int? Level { get; set; } = null;
};

public class GetBTCsubscribedResponse
{
  public IEnumerable<DonorBloodTransferCenterSubscriptionsDTO> BTCsubscribed { get; set; } = null!;
}


public class BTCsubsribed(IMediator _mediator) : Endpoint<GetBTCsubscribedRequest, GetBTCsubscribedResponse>
{
  public override void Configure()
  {
    Get("/BTC/subscribed");
  }
  public override async Task HandleAsync(GetBTCsubscribedRequest req, CancellationToken cancellationToken)
  {
    var res = await _mediator.Send(new GetBTCsubscribedQuery(LoggedUserId: req.LoggedUserId,
      PaginationTake: req.PaginationTake,
      Level: req.Level), cancellationToken);
    if (res.IsSuccess)
    {
      var lwr = new GetBTCsubscribedResponse()
      {
        BTCsubscribed = res.Value
      };
      Response = lwr;
    }
  }

}
