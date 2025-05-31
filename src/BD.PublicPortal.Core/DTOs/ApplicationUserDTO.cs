#nullable disable

using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class ApplicationUserDTO
    {
        #region Constructors

        public ApplicationUserDTO() {
        }

        public ApplicationUserDTO(Guid? donorCorrelationId, bool? donorWantToStayAnonymous, bool? donorExcludeFromPublicPortal, DonorAvailability? donorAvailability, DonorContactMethod? donorContactMethod, string donorName, DateTime donorBirthDate, BloodGroup donorBloodGroup, string donorNIN, string donorTel, string donorNotesForBTC, DateTime? donorLastDonationDate, int? communeId, List<DonorBloodTransferCenterSubscriptionsDTO> donorBloodTransferCenterSubscriptions, CommuneDTO commune) {

          this.DonorCorrelationId = donorCorrelationId;
          this.DonorWantToStayAnonymous = donorWantToStayAnonymous;
          this.DonorExcludeFromPublicPortal = donorExcludeFromPublicPortal;
          this.DonorAvailability = donorAvailability;
          this.DonorContactMethod = donorContactMethod;
          this.DonorName = donorName;
          this.DonorBirthDate = donorBirthDate;
          this.DonorBloodGroup = donorBloodGroup;
          this.DonorNIN = donorNIN;
          this.DonorTel = donorTel;
          this.DonorNotesForBTC = donorNotesForBTC;
          this.DonorLastDonationDate = donorLastDonationDate;
          this.CommuneId = communeId;
          this.DonorBloodTransferCenterSubscriptions = donorBloodTransferCenterSubscriptions;
          this.Commune = commune;
        }

        #endregion

        #region Properties

        public System.Guid? DonorCorrelationId { get; set; }

        public bool? DonorWantToStayAnonymous { get; set; }

        public bool? DonorExcludeFromPublicPortal { get; set; }

        public DonorAvailability? DonorAvailability { get; set; }

        public DonorContactMethod? DonorContactMethod { get; set; }

        public string DonorName { get; set; }

        public System.DateTime DonorBirthDate { get; set; }

        public BloodGroup DonorBloodGroup { get; set; }

        public string DonorNIN { get; set; }

        public string DonorTel { get; set; }

        public string DonorNotesForBTC { get; set; }

        public System.DateTime? DonorLastDonationDate { get; set; }

        public int? CommuneId { get; set; }

        #endregion

        #region Navigation Properties

        public List<DonorBloodTransferCenterSubscriptionsDTO> DonorBloodTransferCenterSubscriptions { get; set; }

        public CommuneDTO Commune { get; set; }

        #endregion
    }


    public class LoginUserCommandResultDTO
    {
      public ApplicationUserDTO UserDTO { get; set; } = null!;
      public string JwToken { get; set; } = null!;

      public Guid UserId { get; set; }
    }


}
