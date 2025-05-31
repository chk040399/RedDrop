#nullable disable

using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class ApplicationUserDTO
    {
        #region Constructors

        public ApplicationUserDTO() {
        }

        public ApplicationUserDTO(Guid id, bool? donorWantToStayAnonymous, bool? donorExcludeFromPublicPortal, DonorAvailability? donorAvailability, DonorContactMethod? donorContactMethod, string donorName, DateTime donorBirthDate, BloodGroup donorBloodGroup, string donorNIN, string donorTel, string email, string donorNotesForBTC, DateTime? donorLastDonationDate, int? communeId, List<DonorBloodTransferCenterSubscriptionsDTO> donorBloodTransferCenterSubscriptions, List<BloodDonationPledgeDTO> bloodDonationPledges, CommuneDTO commune) {

          this.Id = id;
          this.DonorWantToStayAnonymous = donorWantToStayAnonymous;
          this.DonorExcludeFromPublicPortal = donorExcludeFromPublicPortal;
          this.DonorAvailability = donorAvailability;
          this.DonorContactMethod = donorContactMethod;
          this.DonorName = donorName;
          this.DonorBirthDate = donorBirthDate;
          this.DonorBloodGroup = donorBloodGroup;
          this.DonorNIN = donorNIN;
          this.DonorTel = donorTel;
          this.Email = email;
          this.DonorNotesForBTC = donorNotesForBTC;
          this.DonorLastDonationDate = donorLastDonationDate;
          this.CommuneId = communeId;
          this.DonorBloodTransferCenterSubscriptions = donorBloodTransferCenterSubscriptions;
          this.BloodDonationPledges = bloodDonationPledges;
          this.Commune = commune;
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public bool? DonorWantToStayAnonymous { get; set; }

        public bool? DonorExcludeFromPublicPortal { get; set; }

        public DonorAvailability? DonorAvailability { get; set; }

        public DonorContactMethod? DonorContactMethod { get; set; }

        public string DonorName { get; set; }

        public DateTime DonorBirthDate { get; set; }

        public BloodGroup DonorBloodGroup { get; set; }

        public string DonorNIN { get; set; }

        public string DonorTel { get; set; }

        public string Email { get; set; }

        public string DonorNotesForBTC { get; set; }

        public DateTime? DonorLastDonationDate { get; set; }

        public int? CommuneId { get; set; }

        #endregion

        #region Navigation Properties

        public List<DonorBloodTransferCenterSubscriptionsDTO> DonorBloodTransferCenterSubscriptions { get; set; }

        public List<BloodDonationPledgeDTO> BloodDonationPledges { get; set; }

        public CommuneDTO Commune { get; set; }

        #endregion
    }



}
