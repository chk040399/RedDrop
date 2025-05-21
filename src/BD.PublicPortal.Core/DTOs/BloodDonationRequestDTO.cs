#nullable disable

using BD;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class BloodDonationRequestDTO
    {
        #region Constructors

        public BloodDonationRequestDTO() {
        }

        public BloodDonationRequestDTO(Guid id, BloodDonationRequestEvolutionStatus evolutionStatus, BloodDonationType donationType, BloodGroup bloodGroup, int requestedQty, DateTime requestDate, DateTime? requestDueDate, BloodDonationRequestPriority priority, string moreDetails, string serviceName, Guid bloodTansfusionCenterId, List<BloodDonationPledgeDTO> bloodDonationPledges, BloodTansfusionCenterDTO bloodTansfusionCenter) {

      Id = id;
      EvolutionStatus = evolutionStatus;
      DonationType = donationType;
      BloodGroup = bloodGroup;
      RequestedQty = requestedQty;
      RequestDate = requestDate;
      RequestDueDate = requestDueDate;
      Priority = priority;
      MoreDetails = moreDetails;
      ServiceName = serviceName;
      BloodTansfusionCenterId = bloodTansfusionCenterId;
      BloodDonationPledges = bloodDonationPledges;
      BloodTansfusionCenter = bloodTansfusionCenter;
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public BloodDonationRequestEvolutionStatus EvolutionStatus { get; set; }

        public BloodDonationType DonationType { get; set; }

        public BloodGroup BloodGroup { get; set; }

        public int RequestedQty { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? RequestDueDate { get; set; }

        public BloodDonationRequestPriority Priority { get; set; }

        public string MoreDetails { get; set; }

        public string ServiceName { get; set; }

        public Guid BloodTansfusionCenterId { get; set; }

        #endregion

        #region Navigation Properties

        public List<BloodDonationPledgeDTO> BloodDonationPledges { get; set; }

        public BloodTansfusionCenterDTO BloodTansfusionCenter { get; set; }

        #endregion
    }

}
