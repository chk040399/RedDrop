using BD.Central.Core.DTOs;
using BD.Central.Core.Entities.Specifications;
using BD.Central.Application.BTC;

namespace BD.Central.Api.Features.BTC;
public class ListBloodTransfusionCenterRequest
{
  [FromQuery] 
  public BloodTransfusionCenterSpecificationFilter? Filter { get; set; } = null;

  public int? Level { get; set; } = null;
};
  



public class ListBloodTansfusionCentersResponse
{
  public IEnumerable<BloodTansfusionCenterExDTO> BloodTansfusionCenters { get; set; } = null!;
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
