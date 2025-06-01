using BD.PublicPortal.Core.Entities.Enums;
using Domain.ValueObjects;
using System;

namespace BD.BTC.Api.Converters
{
    /// <summary>
    /// Converter between PledgeStatus value objects and BloodDonationPladgeEvolutionStatus enums
    /// </summary>
    public static class PledgeStatusConverter
    {
        /// <summary>
        /// Converts a BloodDonationPladgeEvolutionStatus enum to PledgeStatus value object
        /// </summary>
        public static PledgeStatus ToPledgeStatus(this BloodDonationPladgeEvolutionStatus status)
        {
            return status switch
            {
                BloodDonationPladgeEvolutionStatus.Initiated => PledgeStatus.Pledged,
                BloodDonationPladgeEvolutionStatus.Honored => PledgeStatus.Fulfilled,
                BloodDonationPladgeEvolutionStatus.CanceledByInitiaor => PledgeStatus.Canceled,
                BloodDonationPladgeEvolutionStatus.CanceledByServiceNotNeeded => PledgeStatus.Rejected,
                BloodDonationPladgeEvolutionStatus.CanceledTimeout => PledgeStatus.Expired,
                _ => throw new ArgumentException($"Unsupported pledge status: {status}", nameof(status))
            };
        }

        /// <summary>
        /// Converts PledgeStatus value object back to BloodDonationPladgeEvolutionStatus enum
        /// </summary>
        public static BloodDonationPladgeEvolutionStatus ToEnum(this PledgeStatus status)
        {
            return status.Value switch
            {
                "Pledged" => BloodDonationPladgeEvolutionStatus.Initiated,
                "Fulfilled" => BloodDonationPladgeEvolutionStatus.Honored,
                "Canceled" => BloodDonationPladgeEvolutionStatus.CanceledByInitiaor,
                "Rejected" => BloodDonationPladgeEvolutionStatus.CanceledByServiceNotNeeded,
                "Expired" => BloodDonationPladgeEvolutionStatus.CanceledTimeout,
                _ => throw new ArgumentException($"Unsupported pledge status: {status.Value}", nameof(status))
            };
        }
    }
}