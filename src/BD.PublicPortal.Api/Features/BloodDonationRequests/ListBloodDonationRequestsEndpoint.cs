
using System.Security.Claims;
using BD.PublicPortal.Application.BloodDonationRequests;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;


namespace BD.PublicPortal.Api.Features.BloodDonationRequests;


public class ListBloodDonationRequestsRequest
{
  [FromQuery] 
  public BloodDonationRequestSpecificationFilter? Filter { get; set; } = null;

  [FromClaim(claimType:"UserId",isRequired:false)] 
  public Guid? LoggedUserId { get; set; } = null;
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
    var res = await  _mediator.Send(new ListBloodDonationRequestsQuery(filter:req.Filter,LoggedUserID: req.LoggedUserId,Level:req.Level), cancellationToken);

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
