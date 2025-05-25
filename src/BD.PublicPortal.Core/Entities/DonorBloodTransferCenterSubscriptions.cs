#nullable disable

namespace BD.PublicPortal.Core.Entities
{
    public partial class DonorBloodTransferCenterSubscriptions : EntityBase<Guid>, IAggregateRoot {

        public DonorBloodTransferCenterSubscriptions()
        {
            OnCreated();
        }


        public Guid BloodTansfusionCenterId { get; set; }

        public Guid ApplicationUserId { get; set; }

        public virtual BloodTansfusionCenter BloodTansfusionCenter { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
