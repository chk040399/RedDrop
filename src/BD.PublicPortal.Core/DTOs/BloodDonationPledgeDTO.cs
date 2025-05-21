#nullable disable

using BD;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class BloodDonationPledgeDTO
    {
        #region Constructors

        public BloodDonationPledgeDTO() {
        }

        public BloodDonationPledgeDTO(Guid id, BloodDonationPladgeEvolutionStatus evolutionStatus, DateTime pledgeInitiatedDate, DateTime? pledgeDate, DateTime? pledgeHonoredOrCanceledDate, string pledgeNotes, string cantBeDoneReason, Guid bloodDonationRequestId, string applicationUserId, BloodDonationRequestDTO bloodDonationRequest, ApplicationUserDTO applicationUser) {

      Id = id;
      EvolutionStatus = evolutionStatus;
      PledgeInitiatedDate = pledgeInitiatedDate;
      PledgeDate = pledgeDate;
      PledgeHonoredOrCanceledDate = pledgeHonoredOrCanceledDate;
      PledgeNotes = pledgeNotes;
      CantBeDoneReason = cantBeDoneReason;
      BloodDonationRequestId = bloodDonationRequestId;
      ApplicationUserId = applicationUserId;
      BloodDonationRequest = bloodDonationRequest;
      ApplicationUser = applicationUser;
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public BloodDonationPladgeEvolutionStatus EvolutionStatus { get; set; }

        public DateTime PledgeInitiatedDate { get; set; }

        public DateTime? PledgeDate { get; set; }

        public DateTime? PledgeHonoredOrCanceledDate { get; set; }

        public string PledgeNotes { get; set; }

        public string CantBeDoneReason { get; set; }

        public Guid BloodDonationRequestId { get; set; }

        public string ApplicationUserId { get; set; }

        #endregion

        #region Navigation Properties

        public BloodDonationRequestDTO BloodDonationRequest { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }

        #endregion
    }

}
