﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Devart Entity Developer tool using Data Transfer Object template.
// Code is generated on: 24-05-2025 09:37:07
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace BD.PublicPortal.Core
{

    public partial class ApplicationUserDTO
    {
        #region Constructors

        public ApplicationUserDTO() {
        }

        public ApplicationUserDTO(System.Guid? donorCorrelationId, bool? donorWantToStayAnonymous, bool? donorExcludeFromPublicPortal, DonorAvailability? donorAvailability, DonorContactMethod? donorContactMethod, string donorName, System.DateTime donorBirthDate, BloodGroup donorBloodGroup, string donorNIN, string donorTel, string donorNotesForBTC, System.DateTime? donorLastDonationDate, int? communeId, List<DonorBloodTransferCenterSubscriptionsDTO> donorBloodTransferCenterSubscriptions) {

          this.DonorCorrelationId = donorCorrelationId;
          this.DonorWantToStayAnonymous = donorWantToStayAnonymous;
          this.DonorExcludeFromPublicPortal = donorExcludeFromPublicPortal;
          this.DonorAvailability = donorAvailability;
          this.DonorContactMethod = donorContactMethod;
          this.DonorName = donorName;
          this.DonorBirthDate = donorBirthDate;
          this.DonorBloodGroup = donorBloodGroup;
          this.DonorNIN = donorNIN;
          this.DonorTel = donorTel;
          this.DonorNotesForBTC = donorNotesForBTC;
          this.DonorLastDonationDate = donorLastDonationDate;
          this.CommuneId = communeId;
          this.DonorBloodTransferCenterSubscriptions = donorBloodTransferCenterSubscriptions;
        }

        #endregion

        #region Properties

        public System.Guid? DonorCorrelationId { get; set; }

        public bool? DonorWantToStayAnonymous { get; set; }

        public bool? DonorExcludeFromPublicPortal { get; set; }

        public DonorAvailability? DonorAvailability { get; set; }

        public DonorContactMethod? DonorContactMethod { get; set; }

        public string DonorName { get; set; }

        public System.DateTime DonorBirthDate { get; set; }

        public BloodGroup DonorBloodGroup { get; set; }

        public string DonorNIN { get; set; }

        public string DonorTel { get; set; }

        public string DonorNotesForBTC { get; set; }

        public System.DateTime? DonorLastDonationDate { get; set; }

        public int? CommuneId { get; set; }

        #endregion

        #region Navigation Properties

        public List<DonorBloodTransferCenterSubscriptionsDTO> DonorBloodTransferCenterSubscriptions { get; set; }

        #endregion
    }

}
