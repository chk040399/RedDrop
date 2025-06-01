using BD.Central.Core.Entities;

namespace BD.Central.Core.Entities.Specifications;

public class CommunesSpecifications : Specification<Commune>
{
  
  public CommunesSpecifications(int? wilayaID)
  {
      if (wilayaID != null) Query.Where(c => c.WilayaId == wilayaID);
  }


}
