namespace BD.PublicPortal.Core.Entities.Specifications;

#nullable disable

public record BloodDonationPledgeSpecificationFilter(
  Guid? UserId = null,
  BloodDonationPladgeEvolutionStatus? EvolutionStatus = null,
  int? PaginationTake = null,
  int? PaginationSkip = null);

public class BloodDonationPledgeSpecification : Specification<BloodDonationPledge>
{
  public BloodDonationPledgeSpecification(BloodDonationPledgeSpecificationFilter filter = null,
    Guid? loggedUserId = null, int? level = null)
  {
    if (level > 0)
      Query.Include(x => x.BloodDonationRequest );
    if (filter != null && filter.UserId != null)
      Query.Where(x => x.ApplicationUserId == filter.UserId);
    if (filter != null && filter.EvolutionStatus != null)
      Query.Where(x => x.EvolutionStatus == filter.EvolutionStatus);
    Query.OrderBy(x => x.PledgeInitiatedDate);
    if (filter != null && filter.PaginationTake != null)
      Query.Take((int)filter.PaginationTake);
    if (filter != null && filter.PaginationSkip != null)
      Query.Skip((int)filter.PaginationSkip);
  }
}
