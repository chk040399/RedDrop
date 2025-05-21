#nullable disable

using BD;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Core.DTOs
{

    public static partial class WilayaConverter
    {

        public static WilayaDTO ToDto(this Wilaya source)
        {
            return source.ToDtoWithRelated(0);
        }

        public static WilayaDTO ToDtoWithRelated(this Wilaya source, int level)
        {
            if (source == null)
              return null;

            var target = new WilayaDTO();

            // Properties
            target.Id = source.Id;
            target.Name = source.Name;

            // Navigation Properties
            if (level > 0) {
              target.BloodTansfusionCenters = source.BloodTansfusionCenters.ToDtosWithRelated(level - 1);
              target.Communes = source.Communes.ToDtosWithRelated(level - 1);
            }

            // User-defined partial method
            OnDtoCreating(source, target);

            return target;
        }

        public static Wilaya ToEntity(this WilayaDTO source)
        {
            if (source == null)
              return null;

            var target = new Wilaya();

            // Properties
            target.Id = source.Id;
            target.Name = source.Name;

            // User-defined partial method
            OnEntityCreating(source, target);

            return target;
        }

        public static List<WilayaDTO> ToDtos(this IEnumerable<Wilaya> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDto())
              .ToList();

            return target;
        }

        public static List<WilayaDTO> ToDtosWithRelated(this IEnumerable<Wilaya> source, int level)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDtoWithRelated(level))
              .ToList();

            return target;
        }

        public static List<Wilaya> ToEntities(this IEnumerable<WilayaDTO> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToEntity())
              .ToList();

            return target;
        }

        static partial void OnDtoCreating(Wilaya source, WilayaDTO target);

        static partial void OnEntityCreating(WilayaDTO source, Wilaya target);

    }

}
