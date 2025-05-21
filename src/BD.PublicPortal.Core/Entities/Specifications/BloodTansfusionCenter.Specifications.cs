namespace BD.PublicPortal.Core.Entities.Specifications;

public class BloodTansfusionCenterSpecification : Specification<BloodTansfusionCenter>
{
  
  public BloodTansfusionCenterSpecification(int? wilayaID)
  {
      //Query.Include(c=>c.Commune);
      if (wilayaID != null) Query.Where(c => c.WilayaId == wilayaID);
  }


}
