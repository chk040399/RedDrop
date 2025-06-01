using BD.PublicPortal.Core.Entities.Enums;
using Domain.ValueObjects;
using System;

namespace BD.BTC.Api.Converters
{
    /// <summary>
    /// Converter for Blood Group enum to Blood Type value object and back
    /// </summary>
    public static class BloodGroupConverter
    {
        /// <summary>
        /// Converts BloodGroup enum to BloodType value object
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
                _ => throw new ArgumentException($"Unsupported blood group: {bloodGroup}", nameof(bloodGroup))
            };
        }
        
        /// <summary>
        /// Converts BloodType value object back to BloodGroup enum
        /// </summary>
        public static BloodGroup ToEnum(this BloodType bloodType)
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
                _ => throw new ArgumentException($"Unsupported blood type: {bloodType.Value}", nameof(bloodType))
            };
        }
    }
}