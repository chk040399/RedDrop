using Ardalis.Specification;

namespace BD.PublicPortal.Core.Entities.Specifications;

#nullable disable

public class BloodDonationRequestSpecificationFilter(
  int? wilayaId = null,
  BloodDonationRequestPriority? reqPriority = null,
  BloodGroup? bloodGroup = null,
  BloodDonationType? bloodDonationType = null,
  bool? onlySubscriptions = null,
  List<Guid> userSubscribedCenters = null,
  bool? illigibility = null,
  List<BloodGroup> illigibilityGloups = null,

  int? paginationTake = null,
  int? paginationSkip = null)
{
  public int? WilayaId { get; init; } = wilayaId;
  public BloodDonationRequestPriority? ReqPriority { get; init; } = reqPriority;
  public BloodGroup? BloodGroup { get; init; } = bloodGroup;
  public BloodDonationType? BloodDonationType { get; init; } = bloodDonationType;
  public bool? SubscriptionsOnly { get; init; } = onlySubscriptions;
  public List<Guid> UserSubscribedCenters { get; set; } = userSubscribedCenters;
  public bool? IlligibilityOnly { get; init; } = illigibility;
  public List<BloodGroup> IlligibilityGloups { get; set; } = illigibilityGloups;
  public int? PaginationTake { get; init; } = paginationTake;
  public int? PaginationSkip { get; init; } = paginationSkip;

}

public class BloodDonationRequestSpecification:Specification<BloodDonationRequest>
{
  public BloodDonationRequestSpecification(BloodDonationRequestSpecificationFilter filter = null,Guid?  loggedUserId = null, int? level = null)
  {
    var noResults = ((filter != null) && (filter.SubscriptionsOnly != null && filter.SubscriptionsOnly.Value &&
                                             filter.UserSubscribedCenters != null &&
                                             filter.UserSubscribedCenters.Count == 0)) ;
    if (noResults)
    {
      // no result, no more filter
      Query.Where(x => false);
      return;
    }

    if (level > 0) Query.Include(x => x.BloodTansfusionCenter);

    if (filter != null)
    {
      if (filter.SubscriptionsOnly != null && filter.SubscriptionsOnly.Value &&
          filter.UserSubscribedCenters != null && filter.UserSubscribedCenters.Count > 0)
      {
          Query.Where(x => filter.UserSubscribedCenters.Contains(x.BloodTansfusionCenterId));
      }


      if (filter.IlligibilityOnly != null && filter.IlligibilityOnly.Value && filter.IlligibilityGloups != null)
      {
        Query.Where(x => filter.IlligibilityGloups.Contains(x.BloodGroup));
      }

      if (filter.WilayaId != null)
        Query.Where(x => x.BloodTansfusionCenter.WilayaId == filter.WilayaId);

      if (filter.ReqPriority != null)
        Query.Where(x => x.Priority == filter.ReqPriority);

      if (filter.BloodGroup != null)
        Query.Where(x => x.BloodGroup == filter.BloodGroup);

      if (filter.BloodDonationType != null)
        Query.Where(x => x.DonationType == filter.BloodDonationType);
      
      // pagination

      if (filter.PaginationTake != null)
        Query.Take((int)filter.PaginationTake);

      if (filter.PaginationSkip != null)
        Query.Skip((int)filter.PaginationSkip);
    }

    Query.OrderBy(x => x.BloodTansfusionCenter.WilayaId);

  }
}


