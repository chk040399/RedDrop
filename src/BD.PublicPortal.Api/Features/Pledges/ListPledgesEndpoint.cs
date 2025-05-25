using BD.PublicPortal.Application.Pledges;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Api.Features.Pledges;

public class ListPledgesRequest
{
  [FromQuery]
  public BloodDonationPledgeSpecificationFilter? Filter { get; set; } = null;
  [FromClaim(claimType: "UserId", isRequired: false)]
  public Guid? LoggedUserId { get; set; } = null;
  public int? Level { get; set; } = null;
};

public class ListPledgesResponse
{
  public IEnumerable<BloodDonationPledgeDTO> BloodDonationPledges { get; set; } = null!;
}

public class ListPledgesEndpoint(IMediator _mediator) : Endpoint<ListPledgesRequest, ListPledgesResponse>
{
  
  public override void Configure()
  {
    Get("/Pledges/");
    AllowAnonymous();
  }

  public override async Task HandleAsync(ListPledgesRequest req, CancellationToken cancellationToken)
  {
    var res = await _mediator.Send(new ListPledgesQuery(filter: req.Filter, Level: req.Level), cancellationToken);
    if (res.IsSuccess)
    {
      var lwr = new ListPledgesResponse()
      {
        BloodDonationPledges = res.Value
      };
      Response = lwr;
    }
  }

}
