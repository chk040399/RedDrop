using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Application.Pledges;

public class UpdatePledgeHandler(IRepository<BloodDonationPledge> _pledgeRepository)
    : ICommandHandler<UpdatePledgeCommand, Result<BloodDonationPledgeDTO>>
{
  public async Task<Result<BloodDonationPledgeDTO>> Handle(UpdatePledgeCommand request, CancellationToken cancellationToken)
  {
    // Find the existing pledge  
    var existingPledge = await _pledgeRepository.GetByIdAsync(request.PledgeId, cancellationToken);

    if (existingPledge == null)
    {
      return Result.NotFound();
    }

    // Verify the pledge belongs to the requesting user  
    if (existingPledge.ApplicationUserId != request.ApplicationUserId)
    {
      return Result.Forbidden();
    }

    // Update the pledge properties  
    if (request.EvolutionStatus.HasValue)
    {
      existingPledge.EvolutionStatus = request.EvolutionStatus.Value;

      // Set honored/canceled date when status changes to those states  
      if (request.EvolutionStatus.Value == BloodDonationPladgeEvolutionStatus.CanceledByInitiaor)
      {
        existingPledge.PledgeHonoredOrCanceledDate = DateTime.UtcNow;
      }
    }

    if (request.PledgeDate.HasValue)
    {
      existingPledge.PledgeDate = request.PledgeDate.Value;
    }


    // Save the updated pledge  
    await _pledgeRepository.UpdateAsync(existingPledge, cancellationToken);

    // Convert to DTO and return  
    var pledgeDto = existingPledge.ToDtoWithRelated(1);

    return Result.Success(pledgeDto);
  }
}
