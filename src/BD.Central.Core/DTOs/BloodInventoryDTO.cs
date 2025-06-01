#nullable disable
using BD;
using BD.Central.Core.Entities.Enums;

namespace BD.Central.Core.DTOs
{

    public partial class BloodInventoryDTO
    {
        #region Constructors

        public BloodInventoryDTO() {
        }

        public BloodInventoryDTO(Guid id, Guid bloodTansfusionCenterId, BloodGroup bloodGroup, BloodDonationType bloodDonationType, int? totalQty, int? minQty, int? maxQty, BloodTansfusionCenterDTO bloodTansfusionCenter) {

      Id = id;
      BloodTansfusionCenterId = bloodTansfusionCenterId;
      BloodGroup = bloodGroup;
      BloodDonationType = bloodDonationType;
      TotalQty = totalQty;
      MinQty = minQty;
      MaxQty = maxQty;
      BloodTansfusionCenter = bloodTansfusionCenter;
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public Guid BloodTansfusionCenterId { get; set; }

        public BloodGroup BloodGroup { get; set; }

        public BloodDonationType BloodDonationType { get; set; }

        public int? TotalQty { get; set; }

        public int? MinQty { get; set; }

        public int? MaxQty { get; set; }

        #endregion

        #region Navigation Properties

        public BloodTansfusionCenterDTO BloodTansfusionCenter { get; set; }

        #endregion
    }

}
