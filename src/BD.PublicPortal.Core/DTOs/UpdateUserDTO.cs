#nullable disable

using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class UpdateUserDTO
  {
        #region Constructors

        public UpdateUserDTO() {
        }

        public UpdateUserDTO(bool? donorWantToStayAnonymous, bool? donorExcludeFromPublicPortal, DonorAvailability? donorAvailability, 
          DonorContactMethod? donorContactMethod, string donorName, 
          System.DateTime donorBirthDate, BloodGroup donorBloodGroup, string donorTel, string donorNotesForBTC, System.DateTime? donorLastDonationDate, int? communeId) {

          
          this.DonorWantToStayAnonymous = donorWantToStayAnonymous;
          this.DonorExcludeFromPublicPortal = donorExcludeFromPublicPortal;
          this.DonorAvailability = donorAvailability;
          this.DonorContactMethod = donorContactMethod;
          this.DonorName = donorName;
          this.DonorBirthDate = donorBirthDate;
          this.DonorBloodGroup = donorBloodGroup;
          
          this.DonorTel = donorTel;
          this.DonorNotesForBTC = donorNotesForBTC;
          
          this.CommuneId = communeId;
        }

        #endregion

        #region Properties


        public bool? DonorWantToStayAnonymous { get; set; }

        public bool? DonorExcludeFromPublicPortal { get; set; }

        public DonorAvailability? DonorAvailability { get; set; }

        public DonorContactMethod? DonorContactMethod { get; set; }

        public string DonorName { get; set; }

        public System.DateTime? DonorBirthDate { get; set; }

        public BloodGroup? DonorBloodGroup { get; set; }


        public string DonorTel { get; set; }

        public string DonorNotesForBTC { get; set; }

        public int? CommuneId { get; set; }

        #endregion
    }

}
