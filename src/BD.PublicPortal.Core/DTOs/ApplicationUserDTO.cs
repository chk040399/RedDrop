#nullable disable

using BD;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class ApplicationUserDTO
    {
        #region Constructors

        public ApplicationUserDTO() {
        }

        public ApplicationUserDTO(System.Guid? donorCorrelationId, bool? donorWantToStayAnonymous, bool? donorExcludeFromPublicPortal, DonorAvailability? donorAvailability, DonorContactMethod? donorContactMethod, string donorName, System.DateTime donorBirthDate, BloodGroup donorBloodGroup, string donorNIN, string donorTel, string donorNotesForBTC, System.DateTime? donorLastDonationDate, int? communeId, List<System.Guid> donorBloodTransferCenterSubscriptions, List<System.Guid> bloodDonationPledges) {

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
          this.BloodDonationPledges = bloodDonationPledges;
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

        public List<System.Guid> DonorBloodTransferCenterSubscriptions { get; set; }

        public List<System.Guid> BloodDonationPledges { get; set; }

        #endregion
    }

}
