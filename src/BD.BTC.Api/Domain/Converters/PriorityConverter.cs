using BD.PublicPortal.Core.Entities.Enums;
using Domain.ValueObjects;
using System;

namespace BD.BTC.Api.Converters
{
    /// <summary>
    /// Converter between Priority value objects and BloodDonationRequestPriority enums
    /// </summary>
    public static class PriorityConverter
    {
        /// <summary>
        /// Converts a BloodDonationRequestPriority enum to Priority value object
        /// </summary>
        public static Priority ToPriority(this BloodDonationRequestPriority priority)
        {
            return priority switch
            {
                BloodDonationRequestPriority.Low => Priority.Low(),
                BloodDonationRequestPriority.Normal => Priority.Standard(),
                BloodDonationRequestPriority.Critical => Priority.Critical(),
                _ => throw new ArgumentException($"Unsupported priority: {priority}", nameof(priority))
            };
        }

        /// <summary>
        /// Converts Priority value object back to BloodDonationRequestPriority enum
        /// </summary>
        public static BloodDonationRequestPriority ToEnum(this Priority priority)
        {
            return priority.Value switch
            {
                "low" => BloodDonationRequestPriority.Low,
                "standard" => BloodDonationRequestPriority.Normal,
                "critical" => BloodDonationRequestPriority.Critical,
                _ => throw new ArgumentException($"Unsupported priority: {priority.Value}", nameof(priority))
            };
        }
    }
}