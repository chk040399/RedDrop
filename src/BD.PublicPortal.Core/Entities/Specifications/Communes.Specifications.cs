namespace BD.PublicPortal.Core.Entities.Specifications;

public class CommunesSpecifications : Specification<Commune>
{
  
  public CommunesSpecifications(int? wilayaID)
  {
      if (wilayaID != null) Query.Where(c => c.WilayaId == wilayaID);
  }


}
