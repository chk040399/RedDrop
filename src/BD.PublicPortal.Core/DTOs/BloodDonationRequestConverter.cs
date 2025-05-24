#nullable disable

using BD;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Core.DTOs
{

    public static partial class BloodDonationRequestConverter
    {

        public static BloodDonationRequestDTO ToDto(this BloodDonationRequest source)
        {
            return source.ToDtoWithRelated(0);
        }

        public static BloodDonationRequestDTO ToDtoWithRelated(this BloodDonationRequest source, int level)
        {
            if (source == null)
              return null;

            var target = new BloodDonationRequestDTO();

            // Properties
            target.Id = source.Id;
            target.EvolutionStatus = source.EvolutionStatus;
            target.DonationType = source.DonationType;
            target.BloodGroup = source.BloodGroup;
            target.RequestedQty = source.RequestedQty;
            target.RequestDate = source.RequestDate;
            target.RequestDueDate = source.RequestDueDate;
            target.Priority = source.Priority;
            target.MoreDetails = source.MoreDetails;
            target.ServiceName = source.ServiceName;
            target.BloodTansfusionCenterId = source.BloodTansfusionCenterId;

            // Navigation Properties
            if (level > 0) {
              target.BloodTansfusionCenter = source.BloodTansfusionCenter.ToDtoWithRelated(level - 1);
            }

            // User-defined partial method
            OnDtoCreating(source, target);

            return target;
        }

        public static BloodDonationRequest ToEntity(this BloodDonationRequestDTO source)
        {
            if (source == null)
              return null;

            var target = new BloodDonationRequest();

            // Properties
            target.Id = source.Id;
            target.EvolutionStatus = source.EvolutionStatus;
            target.DonationType = source.DonationType;
            target.BloodGroup = source.BloodGroup;
            target.RequestedQty = source.RequestedQty;
            target.RequestDate = source.RequestDate;
            target.RequestDueDate = source.RequestDueDate;
            target.Priority = source.Priority;
            target.MoreDetails = source.MoreDetails;
            target.ServiceName = source.ServiceName;
            target.BloodTansfusionCenterId = source.BloodTansfusionCenterId;

            // User-defined partial method
            OnEntityCreating(source, target);

            return target;
        }

        public static List<BloodDonationRequestDTO> ToDtos(this IEnumerable<BloodDonationRequest> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDto())
              .ToList();

            return target;
        }

        public static List<BloodDonationRequestDTO> ToDtosWithRelated(this IEnumerable<BloodDonationRequest> source, int level)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDtoWithRelated(level))
              .ToList();

            return target;
        }

        public static List<BloodDonationRequest> ToEntities(this IEnumerable<BloodDonationRequestDTO> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToEntity())
              .ToList();

            return target;
        }

        static partial void OnDtoCreating(BloodDonationRequest source, BloodDonationRequestDTO target);

        static partial void OnEntityCreating(BloodDonationRequestDTO source, BloodDonationRequest target);

    }

}
