using BD.PublicPortal.Core.Entities.Enums;
using Domain.ValueObjects;
using System;

namespace BD.BTC.Api.Converters
{
    /// <summary>
    /// Converter between BloodType value objects and BloodGroup enums
    /// </summary>
    public static class BloodTypeConverter
    {
        /// <summary>
        /// Converts a BloodType value object to a BloodGroup enum
        /// </summary>
        public static BloodGroup ToBloodGroup(this BloodType bloodType)
        {
            return bloodType.Value switch
            {
                "A+" => BloodGroup.A_POSITIVE,
                "A-" => BloodGroup.A_NEGATIVE,
                "B+" => BloodGroup.B_POSITIVE,
                "B-" => BloodGroup.B_NEGATIVE,
                "AB+" => BloodGroup.AB_POSITIVE,
                "AB-" => BloodGroup.AB_NEGATIVE,
                "O+" => BloodGroup.O_POSITIVE,
                "O-" => BloodGroup.O_NEGATIVE,
                _ => throw new ArgumentException($"Invalid blood type: {bloodType.Value}", nameof(bloodType))
            };
        }

        /// <summary>
        /// Converts a BloodGroup enum to a BloodType value object
        /// </summary>
        public static BloodType ToBloodType(this BloodGroup bloodGroup)
        {
            return bloodGroup switch
            {
                BloodGroup.A_POSITIVE => BloodType.APositive(),
                BloodGroup.A_NEGATIVE => BloodType.ANegative(),
                BloodGroup.B_POSITIVE => BloodType.BPositive(),
                BloodGroup.B_NEGATIVE => BloodType.BNegative(),
                BloodGroup.AB_POSITIVE => BloodType.ABPositive(),
                BloodGroup.AB_NEGATIVE => BloodType.ABNegative(),
                BloodGroup.O_POSITIVE => BloodType.OPositive(),
                BloodGroup.O_NEGATIVE => BloodType.ONegative(),
                _ => throw new ArgumentException($"Invalid blood group: {bloodGroup}", nameof(bloodGroup))
            };
        }
    }
}