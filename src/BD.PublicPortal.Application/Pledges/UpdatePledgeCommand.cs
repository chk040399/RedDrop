using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Application.Pledges;

public record UpdatePledgeCommand(
  Guid PledgeId,
  Guid ApplicationUserId,
  BloodDonationPladgeEvolutionStatus? EvolutionStatus = null,
  DateTime? PledgeDate = null,
  string? CantBeDoneReason = null
) : ICommand<Result<BloodDonationPledgeDTO>>;
