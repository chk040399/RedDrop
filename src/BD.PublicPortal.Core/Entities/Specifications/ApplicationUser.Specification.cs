namespace BD.PublicPortal.Core.Entities.Specifications;

#nullable disable

public record ApplicationUserSpecificationFilter(
  int? CommuneId = null,
  int? WilayaId = null,
  BloodGroup? DonorBloodGroup=null,
  DonorContactMethod? DonorContactMethod=null,
  int? PaginationTake = null,
  int? PaginationSkip = null);

public class ApplicationUserSpecificationByLevel : Specification<ApplicationUser>
{

  public ApplicationUserSpecificationByLevel(Guid userGuid,int? level = null)
  {
    if (level > 0)
    {
      Query.Include(x => x.Commune).Include(x => x.Commune.Wilaya);
    }

    Query.Where(u => u.Id == userGuid);


  }
}



public class ApplicationUserSpecification : Specification<ApplicationUser>
  {

    public ApplicationUserSpecification(ApplicationUserSpecificationFilter filter = null, Guid? loggedUserId = null,
      int? level = null)
    {

      if (level > 0)
        Query.Include(x => x.Commune)
          .Include(x => x.Commune.Wilaya);
      if (filter != null && filter.CommuneId != null)
        Query.Where(x => x.CommuneId == filter.CommuneId);
      if (filter != null && filter.WilayaId != null)
        Query.Where(x => x.Commune.WilayaId == filter.WilayaId);


      if (filter != null && filter.DonorBloodGroup != null)
        Query.Where(x => x.DonorBloodGroup == filter.DonorBloodGroup);
      if (filter != null && filter.DonorContactMethod != null)
        Query.Where(x => x.DonorContactMethod == filter.DonorContactMethod);
      Query.OrderBy(x => x.CommuneId);
      if (filter != null && filter.PaginationTake != null)
        Query.Take((int)filter.PaginationTake);
      if (filter != null && filter.PaginationSkip != null)
        Query.Skip((int)filter.PaginationSkip);
    }

    public ApplicationUserSpecification(Guid? ApplicationUserId, bool bIncludeSubscrs = false)
    {
      if (bIncludeSubscrs)
        Query.Include(x => x.DonorBloodTransferCenterSubscriptions);

      if (ApplicationUserId != null)
      {
        Query.Where(x => x.Id == ApplicationUserId);
      }

    }

    public ApplicationUserSpecification(string nin)
    {
      Query.Where(x => x.DonorNIN == nin);
    }
  }

