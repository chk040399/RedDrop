using System.Threading;
using BD.PublicPortal.Application.Donor;
using BD.PublicPortal.Application.Identity;
using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Api.Features.IdentityManagement.Donor;

public class UpdateProfileRequest: UpdateUserDTO
{
  [FromClaim(claimType: "UserId", isRequired: true)]
  public Guid UserId { get; set; }
  
}

public class UpdateProfileResponse
{
  public ApplicationUserDTO? User { get; set; } = null!;
}

public class UpdateProfileEndpoint:Endpoint<UpdateProfileRequest, UpdateProfileResponse>
{
  private readonly IMediator _mediator;
  public UpdateProfileEndpoint(IMediator mediator)
  {
    _mediator = mediator;
  }
  public override void Configure()
  {
    Patch("/donor/profile");
    Summary(s => s.Summary = "Update donor profile.");
  }
  public override async Task HandleAsync(UpdateProfileRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new UpdateProfileCommand(req, req.UserId), ct);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (result.IsSuccess)
    {
      var response = new UpdateProfileResponse
      {
        User = result.Value
      };
      await SendOkAsync(response, ct);
    }
  }

}
