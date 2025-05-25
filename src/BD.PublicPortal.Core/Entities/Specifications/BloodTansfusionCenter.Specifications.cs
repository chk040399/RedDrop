namespace BD.PublicPortal.Core.Entities.Specifications;

#nullable disable

public record BloodTransfusionCenterSpecificationFilter(
  int? WilayaId =null,
  int? PaginationTake = null,
  int? PaginationSkip = null);


public class BloodTansfusionCenterSpecification : Specification<BloodTansfusionCenter>
{

  public BloodTansfusionCenterSpecification(BloodTransfusionCenterSpecificationFilter filter = null,
    Guid? loggedUserId = null, int? level = null)
  {
    if (level > 0)
      Query.Include(x => x.Wilaya);
    if (filter != null && filter.WilayaId != null)
      Query.Where(x => x.WilayaId == filter.WilayaId);
    Query.OrderBy(x => x.WilayaId);
    if (filter != null && filter.PaginationTake != null)
      Query.Take((int)filter.PaginationTake);
    if (filter != null && filter.PaginationSkip != null)
      Query.Skip((int)filter.PaginationSkip);
  }


}
