#nullable disable

using BD;
using BD.Central.Core.Entities.Enums;

namespace BD.Central.Core.DTOs
{

    public partial class UpdateUserDTO
  {
        #region Constructors

        public UpdateUserDTO() {
        }

        public UpdateUserDTO(bool? donorWantToStayAnonymous, bool? donorExcludeFromPublicPortal, DonorAvailability? donorAvailability, 
          DonorContactMethod? donorContactMethod, string donorName,
          DateTime donorBirthDate, BloodGroup donorBloodGroup, string donorTel, string donorNotesForBTC, DateTime? donorLastDonationDate, int? communeId) {


      DonorWantToStayAnonymous = donorWantToStayAnonymous;
      DonorExcludeFromPublicPortal = donorExcludeFromPublicPortal;
      DonorAvailability = donorAvailability;
      DonorContactMethod = donorContactMethod;
      DonorName = donorName;
      DonorBirthDate = donorBirthDate;
      DonorBloodGroup = donorBloodGroup;

      DonorTel = donorTel;
      DonorNotesForBTC = donorNotesForBTC;

      CommuneId = communeId;
        }

        #endregion

        #region Properties


        public bool? DonorWantToStayAnonymous { get; set; }

        public bool? DonorExcludeFromPublicPortal { get; set; }

        public DonorAvailability? DonorAvailability { get; set; }

        public DonorContactMethod? DonorContactMethod { get; set; }

        public string DonorName { get; set; }

        public DateTime? DonorBirthDate { get; set; }

        public BloodGroup? DonorBloodGroup { get; set; }


        public string DonorTel { get; set; }

        public string DonorNotesForBTC { get; set; }

        public int? CommuneId { get; set; }

        #endregion
    }

}
