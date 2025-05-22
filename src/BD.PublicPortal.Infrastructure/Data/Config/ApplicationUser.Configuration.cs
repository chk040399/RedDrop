#nullable disable

using BD.PublicPortal.Core.Entities;


namespace BD.PublicPortal.Infrastructure.Data.Config
{
    /// <summary>
    /// There are no comments for ApplicationUserConfiguration in the schema.
    /// </summary>
    public partial class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<ApplicationUser> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {


            builder.Property(x => x.DonorCorrelationId).HasColumnName(@"DonorCorrelationId").ValueGeneratedNever();
            builder.Property(x => x.DonorWantToStayAnonymous).HasColumnName(@"DonorWantToStayAnonymous").ValueGeneratedNever();
            builder.Property(x => x.DonorExcludeFromPublicPortal).HasColumnName(@"DonorExcludeFromPublicPortal").ValueGeneratedNever();
            builder.Property(x => x.DonorAvailability).HasColumnName(@"DonorAvailability").ValueGeneratedNever();
            builder.Property(x => x.DonorContactMethod).HasColumnName(@"DonorContactMethod").ValueGeneratedNever();
            builder.Property(x => x.DonorName).HasColumnName(@"DonorName").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.DonorBirthDate).HasColumnName(@"DonorBirthDate").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.DonorBloodGroup).HasColumnName(@"DonorBloodGroup").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.DonorNIN).HasColumnName(@"DonorNIN").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.DonorTel).HasColumnName(@"DonorTel").ValueGeneratedNever();
            builder.Property(x => x.DonorNotesForBTC).HasColumnName(@"DonorNotesForBTC").ValueGeneratedNever();
            builder.Property(x => x.DonorLastDonationDate).HasColumnName(@"DonorLastDonationDate").ValueGeneratedNever();
            builder.Property(x => x.CommuneId).HasColumnName(@"CommuneId").ValueGeneratedNever();

            builder.HasMany(x => x.DonorBloodTransferCenterSubscriptions).WithOne(op => op.ApplicationUser).HasForeignKey(@"ApplicationUserId").IsRequired(true);
            builder.HasMany(x => x.BloodDonationPledges).WithOne(op => op.ApplicationUser).HasForeignKey(@"ApplicationUserId").IsRequired(true);
            builder.HasOne(x => x.Commune).WithMany(op => op.ApplicationUsers).HasForeignKey(@"CommuneId").IsRequired(false);


      CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<ApplicationUser> builder);

        #endregion
    }

}
