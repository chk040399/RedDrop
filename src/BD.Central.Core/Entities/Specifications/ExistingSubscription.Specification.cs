using BD.Central.Core.Entities;

namespace BD.Central.Core.Entities.Specifications;

public class ExistingSubscriptionSpecification : Specification<DonorBloodTransferCenterSubscriptions>
{
  public ExistingSubscriptionSpecification(Guid applicationUserId, Guid bloodTansfusionCenterId)
  {
    Query.Where(s => s.ApplicationUserId == applicationUserId &&
                     s.BloodTansfusionCenterId == bloodTansfusionCenterId);
  }
}
