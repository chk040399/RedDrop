
using System.Security.Claims;
using BD.Central.Core.DTOs;
using BD.Central.Core.Entities.Specifications;
using BD.Central.Application.BloodDonationRequests;


namespace BD.Central.Api.Features.BloodDonationRequests;


public class ListBloodDonationRequestsRequest
{
  [FromQuery] 
  public BloodDonationRequestSpecificationFilter? Filter { get; set; } = null;

  public int? Level { get; set; } = null;
};
  



public class ListBloodDonationRequestsResponse
{
  public IEnumerable<BloodDonationRequestDTO> BloodDonationRequests { get; set; } = null!;
}





public class ListBloodDonationRequestsEndpoint(IMediator _mediator) : Endpoint<ListBloodDonationRequestsRequest,ListBloodDonationRequestsResponse>
{
  
public override void Configure()
    {
        Get("BloodDonationRequests");
        AllowAnonymous();        
    }

  public override async Task HandleAsync(ListBloodDonationRequestsRequest req,CancellationToken cancellationToken)
  {
    var res = await  _mediator.Send(new ListBloodDonationRequestsQuery(filter:req.Filter,Level:req.Level), cancellationToken);

    if (res.IsSuccess)
    {
      var lwr = new ListBloodDonationRequestsResponse()
      {
        BloodDonationRequests = res.Value
      };
      Response = lwr;
    }
  }
  
}
