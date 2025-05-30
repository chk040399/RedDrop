using BD.PublicPortal.Application.Pledges;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Api.Features.Pledges;

 public class ListPledgesRequest  
   {  
     [FromQuery]
     public BloodDonationPledgeSpecificationFilter? filter { get; set; } = null;

  [FromClaim(claimType: "UserId", isRequired: true)]  
     public Guid LoggedUserId { get; set; }
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
  }

  public override async Task HandleAsync(ListPledgesRequest req, CancellationToken cancellationToken)
  {
    var res = await _mediator.Send(new ListPledgesQuery(LoggedUserID: req.LoggedUserId,Filter:req.filter  ), cancellationToken);
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
