using BD.PublicPortal.Application.Identity.List;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Api.Features.IdentityManagement.Users.List;

public class ListApplicationUsersRequest
{
  [FromQuery]
  public ApplicationUserSpecificationFilter? Filter { get; set; } = null;
  [FromClaim(claimType: "UserId", isRequired: false)]
  public Guid? LoggedUserId { get; set; } = null;
  public int? Level { get; set; } = null;
};

public class ListApplicationUsersResponse
{
  public IEnumerable<ApplicationUserDTO> Users { get; set; } = null!;
}

public class ListApplicationUsersEndpoint(IMediator _mediator) : Endpoint<ListApplicationUsersRequest, ListApplicationUsersResponse>
{
  public override void Configure()
  {
    Get("/users/");
    AllowAnonymous();
  }
  public override async Task HandleAsync(ListApplicationUsersRequest req, CancellationToken cancellationToken)
  {
    var res = await _mediator.Send(new ListApplicationUsersQuery(filter: req.Filter, Level: req.Level), cancellationToken);
    if (res.IsSuccess)
    {
      var lwr = new ListApplicationUsersResponse()
      {
        Users = res.Value
      };
      Response = lwr;
    }
  }

}
