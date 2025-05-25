#nullable disable



namespace BD.PublicPortal.Core.Entities
{
    public partial class BloodDonationPledge : EntityBase<Guid>, IAggregateRoot {

        public BloodDonationPledge()
        {
            OnCreated();
        }


        public BloodDonationPladgeEvolutionStatus EvolutionStatus { get; set; }

        public DateTime PledgeInitiatedDate { get; set; }

        public DateTime? PledgeDate { get; set; }

        public DateTime? PledgeHonoredOrCanceledDate { get; set; }

        public string PledgeNotes { get; set; }

        public string CantBeDoneReason { get; set; }

        public Guid BloodDonationRequestId { get; set; }

        public Guid ApplicationUserId { get; set; }

        public virtual BloodDonationRequest BloodDonationRequest { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
