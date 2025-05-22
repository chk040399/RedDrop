#nullable disable

using BD.PublicPortal.Core.Entities;


namespace BD.PublicPortal.Infrastructure.Data.Config
{
    /// <summary>
    /// There are no comments for BloodTansfusionCenterConfiguration in the schema.
    /// </summary>
    public partial class BloodTansfusionCenterConfiguration : IEntityTypeConfiguration<BloodTansfusionCenter>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<BloodTansfusionCenter> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<BloodTansfusionCenter> builder)
        {
            builder.ToTable(@"BloodTansfusionCenters");
            builder.Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Name).HasColumnName(@"Name").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Address).HasColumnName(@"Address").ValueGeneratedNever();
            builder.Property(x => x.Contact).HasColumnName(@"Contact").ValueGeneratedNever();
            builder.Property(x => x.Email).HasColumnName(@"Email").ValueGeneratedNever();
            builder.Property(x => x.Tel).HasColumnName(@"Tel").ValueGeneratedNever();
            builder.Property(x => x.WilayaId).HasColumnName(@"WilayaId").ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasMany(x => x.DonorBloodTransferCenterSubscriptions).WithOne(op => op.BloodTansfusionCenter).HasForeignKey(@"BloodTansfusionCenterId").IsRequired(true);
            builder.HasMany(x => x.BloodDonationRequests).WithOne(op => op.BloodTansfusionCenter).HasForeignKey(@"BloodTansfusionCenterId").IsRequired(true);
            builder.HasOne(x => x.Wilaya).WithMany(op => op.BloodTansfusionCenters).OnDelete(DeleteBehavior.Restrict).HasForeignKey(@"WilayaId").IsRequired(true);

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<BloodTansfusionCenter> builder);

        #endregion
    }

}
