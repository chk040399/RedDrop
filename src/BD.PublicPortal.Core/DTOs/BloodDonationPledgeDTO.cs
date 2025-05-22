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

        public BloodDonationPledgeDTO(System.Guid id, BloodDonationPladgeEvolutionStatus evolutionStatus, System.DateTime pledgeInitiatedDate, System.DateTime? pledgeDate, System.DateTime? pledgeHonoredOrCanceledDate, string pledgeNotes, string cantBeDoneReason, System.Guid bloodDonationRequestId, System.Guid applicationUserId) {

          this.Id = id;
          this.EvolutionStatus = evolutionStatus;
          this.PledgeInitiatedDate = pledgeInitiatedDate;
          this.PledgeDate = pledgeDate;
          this.PledgeHonoredOrCanceledDate = pledgeHonoredOrCanceledDate;
          this.PledgeNotes = pledgeNotes;
          this.CantBeDoneReason = cantBeDoneReason;
          this.BloodDonationRequestId = bloodDonationRequestId;
          this.ApplicationUserId = applicationUserId;
        }

        #endregion

        #region Properties

        public System.Guid Id { get; set; }

        public BloodDonationPladgeEvolutionStatus EvolutionStatus { get; set; }

        public System.DateTime PledgeInitiatedDate { get; set; }

        public System.DateTime? PledgeDate { get; set; }

        public System.DateTime? PledgeHonoredOrCanceledDate { get; set; }

        public string PledgeNotes { get; set; }

        public string CantBeDoneReason { get; set; }

        public System.Guid BloodDonationRequestId { get; set; }

        public System.Guid ApplicationUserId { get; set; }

        #endregion
    }

}
