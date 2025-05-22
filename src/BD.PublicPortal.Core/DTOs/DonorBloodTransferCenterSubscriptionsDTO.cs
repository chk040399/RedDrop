#nullable disable


using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class DonorBloodTransferCenterSubscriptionsDTO
    {
        #region Constructors

        public DonorBloodTransferCenterSubscriptionsDTO() {
        }

        public DonorBloodTransferCenterSubscriptionsDTO(System.Guid id, System.Guid bloodTansfusionCenterId, System.Guid applicationUserId) {

          this.Id = id;
          this.BloodTansfusionCenterId = bloodTansfusionCenterId;
          this.ApplicationUserId = applicationUserId;
        }

        #endregion

        #region Properties

        public System.Guid Id { get; set; }

        public System.Guid BloodTansfusionCenterId { get; set; }

        public System.Guid ApplicationUserId { get; set; }

        #endregion
    }

}
