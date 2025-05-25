#nullable disable

namespace BD.PublicPortal.Core.Entities.Specifications;

public record BTCSubscribedSpecificationFilter(
  Guid? ApplicationUserId,
  int? PaginationTake = null,
  int? PaginationSkip = null);

public class BTCSubscribedSpecification: Specification<DonorBloodTransferCenterSubscriptions>
{
  public BTCSubscribedSpecification(BTCSubscribedSpecificationFilter filter,
  Guid? loggedUserId = null, int? level = null)
  {
    if (level >0)
      Query.Include(x => x.BloodTansfusionCenter);
    if (filter != null && filter.ApplicationUserId != null)
      Query.Where(x => x.ApplicationUserId == filter.ApplicationUserId);
    Query.OrderBy(x => x.ApplicationUserId);
    if (filter != null && filter.PaginationTake != null)
      Query.Take((int)filter.PaginationTake);
    if (filter != null && filter.PaginationSkip != null)
      Query.Skip((int)filter.PaginationSkip);
  }
}
