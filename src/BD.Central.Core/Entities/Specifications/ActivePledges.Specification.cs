using BD.Central.Core.Entities;
using BD.Central.Core.Entities.Enums;

namespace BD.Central.Core.Entities.Specifications;

public class ActivePledgesSpecification : Specification<BloodDonationPledge>
{
  public ActivePledgesSpecification()
  {
    // Assuming active pledges are those not cancelled or completed  
    Query.Where(x => x.EvolutionStatus == BloodDonationPladgeEvolutionStatus.Initiated);
  }
}
