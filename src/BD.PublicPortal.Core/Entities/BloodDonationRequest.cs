#nullable disable

using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Core.Entities
{
    public partial class BloodDonationRequest : EntityBase<Guid>, IAggregateRoot {

        public BloodDonationRequest()
        {
            this.BloodDonationPledges = new List<BloodDonationPledge>();
            OnCreated();
        }


        public BloodDonationRequestEvolutionStatus? EvolutionStatus { get; set; }

        public BloodDonationType DonationType { get; set; }

        public BloodGroup BloodGroup { get; set; }

        public int RequestedQty { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? RequestDueDate { get; set; }

        public BloodDonationRequestPriority Priority { get; set; }

        public string MoreDetails { get; set; }

        public string ServiceName { get; set; }

        public Guid BloodTansfusionCenterId { get; set; }

        public virtual IList<BloodDonationPledge> BloodDonationPledges { get; set; }

        public virtual BloodTansfusionCenter BloodTansfusionCenter { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
