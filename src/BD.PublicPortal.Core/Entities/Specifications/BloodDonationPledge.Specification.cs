namespace BD.PublicPortal.Core.Entities.Specifications;

#nullable disable

public record BloodDonationPledgeSpecificationFilter(
  Guid? UserId = null,
  BloodDonationPladgeEvolutionStatus? EvolutionStatus = null,
  int? PaginationTake = null,
  int? PaginationSkip = null);

public class BloodDonationPledgeSpecification : Specification<BloodDonationPledge>
{
  public BloodDonationPledgeSpecification(
    Guid? loggedUserId = null,
    BloodDonationPladgeEvolutionStatus? evolutionStatus = null,
    int? paginationTake = null,
    int? level = null)
  {
    // Filter by the logged user ID  
    if (loggedUserId != null)
    {
      Query.Where(x => x.ApplicationUserId == loggedUserId);
    }

    // Filter by evolution status  
    if (evolutionStatus != null)
    {
      Query.Where(x => x.EvolutionStatus == evolutionStatus);
    }

    // Pagination  
    if (paginationTake != null)
    {
      Query.Take(paginationTake.Value);
    }

    // Include related entities based on level  
    if (level > 0)
    {
      Query.Include(x => x.ApplicationUser)
        .Include(x => x.BloodDonationRequest);
    }

    Query.OrderByDescending(x => x.PledgeInitiatedDate);
  }

  public BloodDonationPledgeSpecification(Guid userId)
  {
    Query.Where(x => x.ApplicationUserId == userId && x.EvolutionStatus==BloodDonationPladgeEvolutionStatus.Initiated);
  }
}

