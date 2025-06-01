#nullable disable

using BD;
using BD.Central.Core.Entities.Enums;

namespace BD.Central.Core.DTOs
{

    public partial class ApplicationUserDTO
    {
        #region Constructors

        public ApplicationUserDTO() {
        }

        public ApplicationUserDTO(Guid id, bool? donorWantToStayAnonymous, bool? donorExcludeFromPublicPortal, DonorAvailability? donorAvailability, DonorContactMethod? donorContactMethod, string donorName, DateTime donorBirthDate, BloodGroup donorBloodGroup, string donorNIN, string donorTel, string email, string donorNotesForBTC, DateTime? donorLastDonationDate, int? communeId, List<DonorBloodTransferCenterSubscriptionsDTO> donorBloodTransferCenterSubscriptions, List<BloodDonationPledgeDTO> bloodDonationPledges, CommuneDTO commune) {

      Id = id;
      DonorWantToStayAnonymous = donorWantToStayAnonymous;
      DonorExcludeFromPublicPortal = donorExcludeFromPublicPortal;
      DonorAvailability = donorAvailability;
      DonorContactMethod = donorContactMethod;
      DonorName = donorName;
      DonorBirthDate = donorBirthDate;
      DonorBloodGroup = donorBloodGroup;
      DonorNIN = donorNIN;
      DonorTel = donorTel;
      Email = email;
      DonorNotesForBTC = donorNotesForBTC;
      DonorLastDonationDate = donorLastDonationDate;
      CommuneId = communeId;
      DonorBloodTransferCenterSubscriptions = donorBloodTransferCenterSubscriptions;
      BloodDonationPledges = bloodDonationPledges;
      Commune = commune;
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
