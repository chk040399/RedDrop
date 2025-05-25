using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Application.Pledges;

public record CreatePledgeCommand(
  Guid ApplicationUserId,
  Guid BloodDonationRequestId,
  DateTime? PledgeDate = null,
  string? PledgeNotes = null
) : ICommand<Result<BloodDonationPledgeDTO>>;
