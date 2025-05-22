#nullable disable

using BD;
using BD.SharedKernel;

namespace BD.PublicPortal.Core.Entities
{
    public partial class BloodTansfusionCenter : EntityBase<Guid>, IAggregateRoot {

        public BloodTansfusionCenter()
        {
            this.DonorBloodTransferCenterSubscriptions = new List<DonorBloodTransferCenterSubscriptions>();
            this.BloodDonationRequests = new List<BloodDonationRequest>();
            OnCreated();
        }


        public string Name { get; set; }

        public string Address { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }

        public int WilayaId { get; set; }

        public virtual IList<DonorBloodTransferCenterSubscriptions> DonorBloodTransferCenterSubscriptions { get; set; }

        public virtual IList<BloodDonationRequest> BloodDonationRequests { get; set; }

        public virtual Wilaya Wilaya { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
