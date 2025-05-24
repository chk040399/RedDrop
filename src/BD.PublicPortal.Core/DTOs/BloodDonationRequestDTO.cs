#nullable disable

using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class BloodDonationRequestDTO
    {
        #region Constructors

        public BloodDonationRequestDTO() {
        }

        public BloodDonationRequestDTO(System.Guid id, BloodDonationRequestEvolutionStatus? evolutionStatus, BloodDonationType donationType, BloodGroup bloodGroup, int requestedQty, System.DateTime requestDate, System.DateTime? requestDueDate, BloodDonationRequestPriority priority, string moreDetails, string serviceName, System.Guid bloodTansfusionCenterId, BloodTansfusionCenterDTO bloodTansfusionCenter) {

          this.Id = id;
          this.EvolutionStatus = evolutionStatus;
          this.DonationType = donationType;
          this.BloodGroup = bloodGroup;
          this.RequestedQty = requestedQty;
          this.RequestDate = requestDate;
          this.RequestDueDate = requestDueDate;
          this.Priority = priority;
          this.MoreDetails = moreDetails;
          this.ServiceName = serviceName;
          this.BloodTansfusionCenterId = bloodTansfusionCenterId;
          this.BloodTansfusionCenter = bloodTansfusionCenter;
        }

        #endregion

        #region Properties

        public System.Guid Id { get; set; }

        public BloodDonationRequestEvolutionStatus? EvolutionStatus { get; set; }

        public BloodDonationType DonationType { get; set; }

        public BloodGroup BloodGroup { get; set; }

        public int RequestedQty { get; set; }

        public System.DateTime RequestDate { get; set; }

        public System.DateTime? RequestDueDate { get; set; }

        public BloodDonationRequestPriority Priority { get; set; }

        public string MoreDetails { get; set; }

        public string ServiceName { get; set; }

        public System.Guid BloodTansfusionCenterId { get; set; }

        #endregion

        #region Navigation Properties

        public BloodTansfusionCenterDTO BloodTansfusionCenter { get; set; }

        #endregion
    }

}
