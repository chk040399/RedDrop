using BD.PublicPortal.Application.BTC;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Api.Features.BTC;
public class ListBloodTransfusionCenterRequest
{
  [FromQuery] 
  public BloodTransfusionCenterSpecificationFilter? Filter { get; set; } = null;

  [FromClaim(claimType:"UserId",isRequired:false)] 
  public Guid? LoggedUserId { get; set; } = null;
  public int? Level { get; set; } = null;
};
  



public class ListBloodTansfusionCentersResponse
{
  public IEnumerable<BloodTansfusionCenterDTO> BloodTansfusionCenters { get; set; } = null!;
}





public class ListBloodDonationRequestsEndpoint(IMediator _mediator) : Endpoint<ListBloodTransfusionCenterRequest, ListBloodTansfusionCentersResponse>
{
  public override void Configure()
  {
    Get("/BTC/");
    AllowAnonymous();
  }

  public override async Task HandleAsync(ListBloodTransfusionCenterRequest req, CancellationToken cancellationToken)
  {
    
    var res = await _mediator.Send(new ListBloodTansfusionCentersQuery(filter:req.Filter,Level:req.Level), cancellationToken);

    if (res.IsSuccess)
    {
      var lwr = new ListBloodTansfusionCentersResponse()
      {
        BloodTansfusionCenters = res.Value
      };
      Response = lwr;
    }
  }
  
}
