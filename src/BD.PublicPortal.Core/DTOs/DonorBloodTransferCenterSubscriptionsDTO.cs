#nullable disable


using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class DonorBloodTransferCenterSubscriptionsDTO
    {
        #region Constructors

        public DonorBloodTransferCenterSubscriptionsDTO() {
        }

        public DonorBloodTransferCenterSubscriptionsDTO(Guid id, Guid bloodTansfusionCenterId, string applicationUserId, BloodTansfusionCenterDTO bloodTansfusionCenter, ApplicationUserDTO applicationUser) {

      Id = id;
      BloodTansfusionCenterId = bloodTansfusionCenterId;
      ApplicationUserId = applicationUserId;
      BloodTansfusionCenter = bloodTansfusionCenter;
      ApplicationUser = applicationUser;
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public Guid BloodTansfusionCenterId { get; set; }

        public string ApplicationUserId { get; set; }

        #endregion

        #region Navigation Properties

        public BloodTansfusionCenterDTO BloodTansfusionCenter { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }

        #endregion
    }

}
