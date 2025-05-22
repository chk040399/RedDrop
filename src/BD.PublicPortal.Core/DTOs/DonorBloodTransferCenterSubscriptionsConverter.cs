#nullable disable


using BD;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Core.DTOs
{

    public static partial class DonorBloodTransferCenterSubscriptionsConverter
    {

        public static DonorBloodTransferCenterSubscriptionsDTO ToDto(this DonorBloodTransferCenterSubscriptions source)
        {
            return source.ToDtoWithRelated(0);
        }

        public static DonorBloodTransferCenterSubscriptionsDTO ToDtoWithRelated(this DonorBloodTransferCenterSubscriptions source, int level)
        {
            if (source == null)
              return null;

            var target = new DonorBloodTransferCenterSubscriptionsDTO();

            // Properties
            target.Id = source.Id;
            target.BloodTansfusionCenterId = source.BloodTansfusionCenterId;
            target.ApplicationUserId = source.ApplicationUserId;

            // User-defined partial method
            OnDtoCreating(source, target);

            return target;
        }

        public static DonorBloodTransferCenterSubscriptions ToEntity(this DonorBloodTransferCenterSubscriptionsDTO source)
        {
            if (source == null)
              return null;

            var target = new DonorBloodTransferCenterSubscriptions();

            // Properties
            target.Id = source.Id;
            target.BloodTansfusionCenterId = source.BloodTansfusionCenterId;
            target.ApplicationUserId = source.ApplicationUserId;

            // User-defined partial method
            OnEntityCreating(source, target);

            return target;
        }

        public static List<DonorBloodTransferCenterSubscriptionsDTO> ToDtos(this IEnumerable<DonorBloodTransferCenterSubscriptions> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDto())
              .ToList();

            return target;
        }

        public static List<DonorBloodTransferCenterSubscriptionsDTO> ToDtosWithRelated(this IEnumerable<DonorBloodTransferCenterSubscriptions> source, int level)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDtoWithRelated(level))
              .ToList();

            return target;
        }

        public static List<DonorBloodTransferCenterSubscriptions> ToEntities(this IEnumerable<DonorBloodTransferCenterSubscriptionsDTO> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToEntity())
              .ToList();

            return target;
        }

        static partial void OnDtoCreating(DonorBloodTransferCenterSubscriptions source, DonorBloodTransferCenterSubscriptionsDTO target);

        static partial void OnEntityCreating(DonorBloodTransferCenterSubscriptionsDTO source, DonorBloodTransferCenterSubscriptions target);

    }

}
