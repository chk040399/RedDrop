using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Pledges;

public class CreatePledgeHandler(IRepository<BloodDonationPledge> _pledgeRepository)
  : ICommandHandler<CreatePledgeCommand, Result<BloodDonationPledgeDTO>>
{
  public async Task<Result<BloodDonationPledgeDTO>> Handle(CreatePledgeCommand request, CancellationToken cancellationToken)
  {

    var existingInitiatedPldgeCount = await
      _pledgeRepository.CountAsync(new BloodDonationPledgeSpecification(request.ApplicationUserId));

    if (existingInitiatedPldgeCount > 0)
    {
      return Result.Invalid(new ValidationError("You already have an active pledge. Please complete or cancel it before creating a new one."));
    }

      // Create new pledge entity  
      var pledge = new BloodDonationPledge
    {
      BloodDonationRequestId = request.BloodDonationRequestId,
      ApplicationUserId = request.ApplicationUserId,
      PledgeInitiatedDate = DateTime.UtcNow,
      PledgeDate = request.PledgeDate,
      PledgeNotes = request.PledgeNotes,
      EvolutionStatus = BloodDonationPladgeEvolutionStatus.Initiated // Default status  
    };

    // Save to repository  
    var savedPledge = await _pledgeRepository.AddAsync(pledge, cancellationToken);

    // Convert to DTO and return  
    var pledgeDto = savedPledge.ToDtoWithRelated(1);

    return Result.Success(pledgeDto);
  }
}
