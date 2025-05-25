using BD.PublicPortal.Application.Pledges;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;
using FluentValidation;

namespace BD.PublicPortal.Api.Features.Pledges;

public class UpdatePledgeRequest
{
  [FromClaim(claimType: "UserId", isRequired: false)]
  public Guid PledgeId { get; set; }
  public BloodDonationPladgeEvolutionStatus? EvolutionStatus { get; set; }
  public DateTime? PledgeDate { get; set; }
  public Guid ApplicationUserId { get; set; }
}

public class UpdatePledgeResponse
{
  public BloodDonationPledgeDTO BloodDonationPledge { get; set; } = null!;
}

public class UpdatePledgeValidator : Validator<UpdatePledgeRequest>
{
  public UpdatePledgeValidator()
  {
    RuleFor(x => x.PledgeId).NotEmpty();
    RuleFor(x => x.ApplicationUserId).NotEmpty();

    RuleFor(x => x)
      .Must(x => x.EvolutionStatus.HasValue || x.PledgeDate.HasValue)
      .WithMessage("At least one field must be provided for update");
    RuleFor(x => x.EvolutionStatus)
      .Must(x => x == BloodDonationPladgeEvolutionStatus.CanceledByInitiaor || !x.HasValue)
      .WithMessage("Only The status 'CanceledByInitiaor' is allowed for update");
    RuleFor(x => x.PledgeDate)
      .Must(x => x > DateTime.UtcNow)
      .WithMessage("The pledge date must be in the future");
  }
}

public class UpdatePledgeEndpoint(IMediator _mediator) : Endpoint<UpdatePledgeRequest, UpdatePledgeResponse>
{
  public override void Configure()
  {
    Put("/Pledges/{PledgeId:guid}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(UpdatePledgeRequest req, CancellationToken cancellationToken)
  {
    var command = new UpdatePledgeCommand(
      req.PledgeId,
      req.ApplicationUserId,
      req.EvolutionStatus,
      req.PledgeDate
    );

    var result = await _mediator.Send(command, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new UpdatePledgeResponse()
      {
        BloodDonationPledge = result.Value
      };
    }
  }
}
