using BD.BloodCentral.Core;

namespace BD.PublicPortal.Core.Entities.Specifications;

public class BloodInventorySpecification : Specification<BloodInventory>
{
  public BloodInventorySpecification(int level = 0)
  {
    if (level > 0)
    {
      //AddInclude(x => x.BloodTansfusionCenter);
      //if (level > 1)
      //{
      //  AddInclude("BloodTansfusionCenter.Wilaya");
      //}
      Query.Include(x => x.BloodTansfusionCenter)
           .Include(x => x.BloodTansfusionCenter.Wilaya);
    }
  }
}
