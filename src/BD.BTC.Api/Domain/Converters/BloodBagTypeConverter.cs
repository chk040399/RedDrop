using BD.PublicPortal.Core.Entities.Enums;
using Domain.ValueObjects;
using System;

namespace BD.BTC.Api.Converters
{
    /// <summary>
    /// Converter between BloodBagType value objects and BloodDonationType enums
    /// </summary>
    public static class BloodBagTypeConverter
    {
        /// <summary>
        /// Converts a BloodDonationType enum to BloodBagType value object
        /// </summary>
        public static BloodBagType ToBloodBagType(this BloodDonationType donationType)
        {
            return donationType switch
            {
                BloodDonationType.WholeBlood => BloodBagType.Blood(),
                BloodDonationType.Platelet => BloodBagType.Plaquette(),
                BloodDonationType.Plasma => BloodBagType.Plasma(),
       
                _ => throw new ArgumentException($"Unsupported donation type: {donationType}", nameof(donationType))
            };
        }

        /// <summary>
        /// Converts BloodBagType value object back to BloodDonationType enum
        /// </summary>
        public static BloodDonationType ToEnum(this BloodBagType bloodBagType)
        {
            return bloodBagType.Value switch
            {
                "blood" => BloodDonationType.WholeBlood,
                "platelets" => BloodDonationType.Platelet,
                "plasma" => BloodDonationType.Plasma,
                _ => throw new ArgumentException($"Unsupported blood bag type: {bloodBagType.Value}", nameof(bloodBagType))
            };
        }
    }
}