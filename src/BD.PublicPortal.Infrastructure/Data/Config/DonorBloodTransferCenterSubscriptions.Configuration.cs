#nullable disable

using BD.PublicPortal.Core.Entities;


namespace BD.PublicPortal.Infrastructure.Data.Config
{
    /// <summary>
    /// There are no comments for DonorBloodTransferCenterSubscriptionsConfiguration in the schema.
    /// </summary>
    public partial class DonorBloodTransferCenterSubscriptionsConfiguration : IEntityTypeConfiguration<DonorBloodTransferCenterSubscriptions>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<DonorBloodTransferCenterSubscriptions> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<DonorBloodTransferCenterSubscriptions> builder)
        {
            builder.ToTable(@"DonorBloodTransferCenterSubscriptions");
            builder.Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.BloodTansfusionCenterId).HasColumnName(@"BloodTansfusionCenterId").ValueGeneratedNever();
            builder.Property(x => x.ApplicationUserId).HasColumnName(@"ApplicationUserId").ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasOne(x => x.BloodTansfusionCenter).WithMany(op => op.DonorBloodTransferCenterSubscriptions).HasForeignKey(@"BloodTansfusionCenterId").IsRequired(true);
            builder.HasOne(x => x.ApplicationUser).WithMany(op => op.DonorBloodTransferCenterSubscriptions).HasForeignKey(@"ApplicationUserId").IsRequired(true);

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<DonorBloodTransferCenterSubscriptions> builder);

        #endregion
    }

}
