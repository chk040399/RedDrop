#nullable disable

using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class DonorBloodTransferCenterSubscriptionsDTO
    {
        #region Constructors

        public DonorBloodTransferCenterSubscriptionsDTO() {
        }

        public DonorBloodTransferCenterSubscriptionsDTO(System.Guid id, System.Guid bloodTansfusionCenterId, System.Guid applicationUserId, BloodTansfusionCenterDTO bloodTansfusionCenter, ApplicationUserDTO applicationUser) {

          this.Id = id;
          this.BloodTansfusionCenterId = bloodTansfusionCenterId;
          this.ApplicationUserId = applicationUserId;
          this.BloodTansfusionCenter = bloodTansfusionCenter;
          this.ApplicationUser = applicationUser;
        }

        #endregion

        #region Properties

        public System.Guid Id { get; set; }

        public System.Guid BloodTansfusionCenterId { get; set; }

        public System.Guid ApplicationUserId { get; set; }

        #endregion

        #region Navigation Properties

        public BloodTansfusionCenterDTO BloodTansfusionCenter { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }

        #endregion
    }

}
