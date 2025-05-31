#nullable disable

namespace BD.BloodCentral.Core
{
    public partial class BloodInventory : EntityBase<Guid>, IAggregateRoot {

        public BloodInventory()
        {
            OnCreated();
        }


        public Guid BloodTansfusionCenterId { get; set; }

        public BloodGroup BloodGroup { get; set; }

        public BloodDonationType BloodDonationType { get; set; }

        public int? TotalQty { get; set; }

        public int? MinQty { get; set; }

        public int? MaxQty { get; set; }


        public virtual BloodTansfusionCenter BloodTansfusionCenter { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
