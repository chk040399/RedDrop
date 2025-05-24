#nullable disable

using BD;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Core.DTOs
{

    public static partial class BloodTansfusionCenterConverter
    {

        public static BloodTansfusionCenterDTO ToDto(this BloodTansfusionCenter source)
        {
            return source.ToDtoWithRelated(0);
        }

        public static BloodTansfusionCenterDTO ToDtoWithRelated(this BloodTansfusionCenter source, int level)
        {
            if (source == null)
              return null;

            var target = new BloodTansfusionCenterDTO();

            // Properties
            target.Id = source.Id;
            target.Name = source.Name;
            target.Address = source.Address;
            target.Contact = source.Contact;
            target.Email = source.Email;
            target.Tel = source.Tel;
            target.WilayaId = source.WilayaId;

            // Navigation Properties
            if (level > 0) {
              target.DonorBloodTransferCenterSubscriptions = source.DonorBloodTransferCenterSubscriptions.ToDtosWithRelated(level - 1);
              target.BloodDonationRequests = source.BloodDonationRequests.ToDtosWithRelated(level - 1);
              target.Wilaya = source.Wilaya.ToDtoWithRelated(level - 1);
            }

            // User-defined partial method
            OnDtoCreating(source, target);

            return target;
        }

        public static BloodTansfusionCenter ToEntity(this BloodTansfusionCenterDTO source)
        {
            if (source == null)
              return null;

            var target = new BloodTansfusionCenter();

            // Properties
            target.Id = source.Id;
            target.Name = source.Name;
            target.Address = source.Address;
            target.Contact = source.Contact;
            target.Email = source.Email;
            target.Tel = source.Tel;
            target.WilayaId = source.WilayaId;

            // User-defined partial method
            OnEntityCreating(source, target);

            return target;
        }

        public static List<BloodTansfusionCenterDTO> ToDtos(this IEnumerable<BloodTansfusionCenter> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDto())
              .ToList();

            return target;
        }

        public static List<BloodTansfusionCenterDTO> ToDtosWithRelated(this IEnumerable<BloodTansfusionCenter> source, int level)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDtoWithRelated(level))
              .ToList();

            return target;
        }

        public static List<BloodTansfusionCenter> ToEntities(this IEnumerable<BloodTansfusionCenterDTO> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToEntity())
              .ToList();

            return target;
        }

        static partial void OnDtoCreating(BloodTansfusionCenter source, BloodTansfusionCenterDTO target);

        static partial void OnEntityCreating(BloodTansfusionCenterDTO source, BloodTansfusionCenter target);

    }

}
