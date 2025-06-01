#nullable disable

using BD;

namespace BD.Central.Core.DTOs
{

    public partial class BloodTansfusionCenterDTO
    {
        #region Constructors

        public BloodTansfusionCenterDTO() {
        }

        public BloodTansfusionCenterDTO(Guid id, string name, string address, string contact, string email, string tel, int wilayaId, List<DonorBloodTransferCenterSubscriptionsDTO> donorBloodTransferCenterSubscriptions, List<BloodDonationRequestDTO> bloodDonationRequests, WilayaDTO wilaya, List<BloodInventoryDTO> bloodInventories) {

      Id = id;
      Name = name;
      Address = address;
      Contact = contact;
      Email = email;
      Tel = tel;
      WilayaId = wilayaId;
      DonorBloodTransferCenterSubscriptions = donorBloodTransferCenterSubscriptions;
      BloodDonationRequests = bloodDonationRequests;
      Wilaya = wilaya;
      BloodInventories = bloodInventories;
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }

        public int WilayaId { get; set; }

        #endregion

        #region Navigation Properties

        public List<DonorBloodTransferCenterSubscriptionsDTO> DonorBloodTransferCenterSubscriptions { get; set; }

        public List<BloodDonationRequestDTO> BloodDonationRequests { get; set; }

        public WilayaDTO Wilaya { get; set; }
		
		public List<BloodInventoryDTO> BloodInventories { get; set; }

        #endregion
    }

    public partial class BloodTansfusionCenterExDTO : BloodTansfusionCenterDTO
  {
    #region Constructors
    public BloodTansfusionCenterExDTO(bool? loggedUserSubscribed)
    {
      LoggedUserSubscribed = loggedUserSubscribed;
    }
	
	

    public bool? LoggedUserSubscribed { get; set; } = null;

    #endregion
  }
}
