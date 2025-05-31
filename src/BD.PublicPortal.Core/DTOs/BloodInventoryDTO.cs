#nullable disable
using BD.PublicPortal.Core.DTOs;

namespace BD.BloodCentral.Core
{

    public partial class BloodInventoryDTO
    {
        #region Constructors

        public BloodInventoryDTO() {
        }

        public BloodInventoryDTO(Guid id, Guid bloodTansfusionCenterId, BloodGroup bloodGroup, BloodDonationType bloodDonationType, int? totalQty, int? minQty, int? maxQty, BloodTansfusionCenterDTO bloodTansfusionCenter) {

          this.Id = id;
          this.BloodTansfusionCenterId = bloodTansfusionCenterId;
          this.BloodGroup = bloodGroup;
          this.BloodDonationType = bloodDonationType;
          this.TotalQty = totalQty;
          this.MinQty = minQty;
          this.MaxQty = maxQty;
          this.BloodTansfusionCenter = bloodTansfusionCenter;
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
