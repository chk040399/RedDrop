using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSTS_Back.Migrations
{
    /// <inheritdoc />
    public partial class FixBloodTransferCenterConstraintFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove the check constraint if it exists
            migrationBuilder.Sql("ALTER TABLE \"BloodTransferCenters\" DROP CONSTRAINT IF EXISTS \"CK_SingleBloodTransferCenter\";");
            
            // Make sure the SingletonCheck column exists (conditionally)
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                                  WHERE table_name = 'BloodTransferCenters' 
                                  AND column_name = 'SingletonCheck') THEN
                        ALTER TABLE ""BloodTransferCenters"" ADD COLUMN ""SingletonCheck"" INTEGER NOT NULL DEFAULT 1;
                    END IF;
                END $$;
            ");
            
            // Make sure the unique index exists (conditionally)
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_indexes 
                                  WHERE tablename = 'BloodTransferCenters' 
                                  AND indexname = 'IX_BloodTransferCenters_SingletonCheck') THEN
                        CREATE UNIQUE INDEX ""IX_BloodTransferCenters_SingletonCheck"" ON ""BloodTransferCenters"" (""SingletonCheck"");
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
