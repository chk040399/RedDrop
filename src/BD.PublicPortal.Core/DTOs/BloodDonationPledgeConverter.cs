#nullable disable

using BD;
using BD.PublicPortal.Core.Entities;

namespace BD.PublicPortal.Core.DTOs
{

    public static partial class BloodDonationPledgeConverter
    {

        public static BloodDonationPledgeDTO ToDto(this BloodDonationPledge source)
        {
            return source.ToDtoWithRelated(0);
        }

        public static BloodDonationPledgeDTO ToDtoWithRelated(this BloodDonationPledge source, int level)
        {
            if (source == null)
              return null;

            var target = new BloodDonationPledgeDTO();

            // Properties
            target.Id = source.Id;
            target.EvolutionStatus = source.EvolutionStatus;
            target.PledgeInitiatedDate = source.PledgeInitiatedDate;
            target.PledgeDate = source.PledgeDate;
            target.PledgeHonoredOrCanceledDate = source.PledgeHonoredOrCanceledDate;
            target.PledgeNotes = source.PledgeNotes;
            target.CantBeDoneReason = source.CantBeDoneReason;
            target.BloodDonationRequestId = source.BloodDonationRequestId;
            target.ApplicationUserId = source.ApplicationUserId;

            // User-defined partial method
            OnDtoCreating(source, target);

            return target;
        }

        public static BloodDonationPledge ToEntity(this BloodDonationPledgeDTO source)
        {
            if (source == null)
              return null;

            var target = new BloodDonationPledge();

            // Properties
            target.Id = source.Id;
            target.EvolutionStatus = source.EvolutionStatus;
            target.PledgeInitiatedDate = source.PledgeInitiatedDate;
            target.PledgeDate = source.PledgeDate;
            target.PledgeHonoredOrCanceledDate = source.PledgeHonoredOrCanceledDate;
            target.PledgeNotes = source.PledgeNotes;
            target.CantBeDoneReason = source.CantBeDoneReason;
            target.BloodDonationRequestId = source.BloodDonationRequestId;
            target.ApplicationUserId = source.ApplicationUserId;

            // User-defined partial method
            OnEntityCreating(source, target);

            return target;
        }

        public static List<BloodDonationPledgeDTO> ToDtos(this IEnumerable<BloodDonationPledge> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDto())
              .ToList();

            return target;
        }

        public static List<BloodDonationPledgeDTO> ToDtosWithRelated(this IEnumerable<BloodDonationPledge> source, int level)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDtoWithRelated(level))
              .ToList();

            return target;
        }

        public static List<BloodDonationPledge> ToEntities(this IEnumerable<BloodDonationPledgeDTO> source)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToEntity())
              .ToList();

            return target;
        }

        static partial void OnDtoCreating(BloodDonationPledge source, BloodDonationPledgeDTO target);

        static partial void OnEntityCreating(BloodDonationPledgeDTO source, BloodDonationPledge target);

    }

}
