using BD.PublicPortal.Core.Entities.Enums;
using Domain.ValueObjects;
using System;

namespace BD.BTC.Api.Converters
{
    /// <summary>
    /// Converter between RequestStatus value objects and BloodDonationRequestEvolutionStatus enums
    /// </summary>
    public static class RequestStatusConverter
    {
        /// <summary>
        /// Converts a BloodDonationRequestEvolutionStatus enum to RequestStatus value object
        /// </summary>
        public static RequestStatus ToRequestStatus(this BloodDonationRequestEvolutionStatus status)
        {
            return status switch
            {
                BloodDonationRequestEvolutionStatus.Waiting => RequestStatus.Pending(),
                BloodDonationRequestEvolutionStatus.PartiallyResolved => RequestStatus.Partial(),
                BloodDonationRequestEvolutionStatus.Resolved => RequestStatus.Resolved(),
                BloodDonationRequestEvolutionStatus.Canceled => RequestStatus.Cancelled(),
                _ => throw new ArgumentException($"Unsupported request status: {status}", nameof(status))
            };
        }

        /// <summary>
        /// Converts RequestStatus value object back to BloodDonationRequestEvolutionStatus enum
        /// </summary>
        public static BloodDonationRequestEvolutionStatus ToEnum(this RequestStatus status)
        {
            return status.Value switch
            {
                "pending" => BloodDonationRequestEvolutionStatus.Waiting,
                "partial" => BloodDonationRequestEvolutionStatus.PartiallyResolved,
                "resolved" => BloodDonationRequestEvolutionStatus.Resolved,
                "cancelled" => BloodDonationRequestEvolutionStatus.Canceled,
                "canceled" => BloodDonationRequestEvolutionStatus.Canceled,
                _ => throw new ArgumentException($"Unsupported request status: {status.Value}", nameof(status))
            };
        }
    }
}