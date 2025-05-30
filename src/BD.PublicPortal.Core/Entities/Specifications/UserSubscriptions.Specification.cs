namespace BD.PublicPortal.Core.Entities.Specifications;

public class UserSubscriptionsSpecification : Specification<DonorBloodTransferCenterSubscriptions>
{
  public UserSubscriptionsSpecification(
    Guid? userId = null,
    int? paginationTake = null,
    int? level = null)
  {
    if (userId != null)
    {
      Query.Where(x => x.ApplicationUserId == userId);
    }

    if (paginationTake != null)
    {
      Query.Take(paginationTake.Value);
    }

    if (level > 0)
    {
      Query.Include(x => x.BloodTansfusionCenter)
        .Include(x => x.ApplicationUser)
        .Include(x=> x.BloodTansfusionCenter.Wilaya);
    }

    Query.OrderBy(x => x.BloodTansfusionCenter.Name);
  }

  public UserSubscriptionsSpecification(Guid userGuid)
  {
    Query.Where(x => x.ApplicationUserId == userGuid);
  }
}
