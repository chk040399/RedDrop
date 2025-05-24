#nullable disable

using BD;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Core.DTOs
{

    public static partial class ApplicationUserConverter
    {

        public static ApplicationUserDTO ToDto(this ApplicationUser source)
        {
            return source.ToDtoWithRelated(0);
        }

        public static ApplicationUserDTO ToDtoWithRelated(this ApplicationUser source, int level)
        {
            if (source == null)
              return null;

            var target = new ApplicationUserDTO();

            // Properties
            target.DonorCorrelationId = source.DonorCorrelationId;
            target.DonorWantToStayAnonymous = source.DonorWantToStayAnonymous;
            target.DonorExcludeFromPublicPortal = source.DonorExcludeFromPublicPortal;
            target.DonorAvailability = source.DonorAvailability;
            target.DonorContactMethod = source.DonorContactMethod;
            target.DonorName = source.DonorName;
            target.DonorBirthDate = source.DonorBirthDate;
            target.DonorBloodGroup = source.DonorBloodGroup;
            target.DonorNIN = source.DonorNIN;
            target.DonorTel = source.DonorTel;
            target.DonorNotesForBTC = source.DonorNotesForBTC;
            target.DonorLastDonationDate = source.DonorLastDonationDate;
            target.CommuneId = source.CommuneId;

            // User-defined partial method
            OnDtoCreating(source, target);

            return target;
        }

        public static ApplicationUser ToEntity(this ApplicationUserDTO source)
        {
            if (source == null)
              return null;

            var target = new ApplicationUser();

            // Properties
            target.DonorCorrelationId = source.DonorCorrelationId;
            target.DonorWantToStayAnonymous = source.DonorWantToStayAnonymous;
            target.DonorExcludeFromPublicPortal = source.DonorExcludeFromPublicPortal;
            target.DonorAvailability = source.DonorAvailability;
            target.DonorContactMethod = source.DonorContactMethod;
            target.DonorName = source.DonorName;
            target.DonorBirthDate = source.DonorBirthDate;
            target.DonorBloodGroup = source.DonorBloodGroup;
            target.DonorNIN = source.DonorNIN;
            target.DonorTel = source.DonorTel;
            target.DonorNotesForBTC = source.DonorNotesForBTC;
            target.DonorLastDonationDate = source.DonorLastDonationDate;
            target.CommuneId = source.CommuneId;

            // User-defined partial method
            OnEntityCreating(source, target);

            return target;
        }

        public static List<ApplicationUserDTO> ToDtos(this IEnumerable<ApplicationUser> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDto())
              .ToList();

            return target;
        }

        public static List<ApplicationUserDTO> ToDtosWithRelated(this IEnumerable<ApplicationUser> source, int level)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDtoWithRelated(level))
              .ToList();

            return target;
        }

        public static List<ApplicationUser> ToEntities(this IEnumerable<ApplicationUserDTO> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToEntity())
              .ToList();

            return target;
        }

        static partial void OnDtoCreating(ApplicationUser source, ApplicationUserDTO target);

        static partial void OnEntityCreating(ApplicationUserDTO source, ApplicationUser target);

    }

}
