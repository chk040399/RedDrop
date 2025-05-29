using BD.PublicPortal.Application.Identity;
using BD.PublicPortal.Core.DTOs;


namespace BD.PublicPortal.Api.Features.IdentityManagement.Users.GetById;

public class GetUserByIdEndpointRequest
{
  [FromClaim(claimType: "UserId", isRequired: false)]
  public Guid UserId { get; set; }
};

public class GetUserByIdEndpointResponse
{
  public ApplicationUserDTO? User { get; set; } = null!;
}

public class GetUserByIdEndpoint(IMediator _mediator) : Endpoint<GetUserByIdEndpointRequest, GetUserByIdEndpointResponse>
{
  public override void Configure()
  {
    Get("/user/");
  }
  public override async Task HandleAsync(GetUserByIdEndpointRequest req, CancellationToken cancellationToken)
  {
    var res = await _mediator.Send(new GetUserByIdQuery(req.UserId), cancellationToken);
    if (res.IsSuccess)
    {
      var lwr = new GetUserByIdEndpointResponse()
      {
        User = res.Value
      };
      Response = lwr;
    }
  }
}
