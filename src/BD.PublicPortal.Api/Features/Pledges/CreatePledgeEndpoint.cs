using BD.PublicPortal.Application.Pledges;
using BD.PublicPortal.Core.DTOs;
using FluentValidation;

namespace BD.PublicPortal.Api.Features.Pledges;

public class CreatePledgeRequest
{

  [FromClaim(claimType: "UserId", isRequired: false)]
  public Guid ApplicationUserId { get; set; }
  public Guid BloodDonationRequestId { get; set; }
  public DateTime? PledgeDate { get; set; }
  public string? PledgeNotes { get; set; }

}

public class CreatePledgeResponse
{
  public BloodDonationPledgeDTO BloodDonationPledge { get; set; } = null!;
}

public class CreatePledgeValidator : Validator<CreatePledgeRequest>
{
  public CreatePledgeValidator()
  {
    RuleFor(x => x.BloodDonationRequestId).NotEmpty();
    RuleFor(x => x.ApplicationUserId).NotEmpty();
  }
}

public class CreatePledgeEndpoint(IMediator _mediator) : Endpoint<CreatePledgeRequest, CreatePledgeResponse>
{
  public override void Configure()
  {
    Post("/Pledges/");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreatePledgeRequest req, CancellationToken cancellationToken)
  {
    var command = new CreatePledgeCommand(
      
      req.ApplicationUserId,
      req.BloodDonationRequestId,
      req.PledgeDate,
      req.PledgeNotes
    );

    var res = await _mediator.Send(command, cancellationToken);

    if (res.IsSuccess)
    {
      Response = new CreatePledgeResponse()
      {
        BloodDonationPledge = res.Value
      };
    }
    // Handle error cases as needed  
  }
}
