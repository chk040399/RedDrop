using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSTS_Back.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteAndUpdatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BloodBags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddCheckConstraint(
                name: "CK_SingleBloodTransferCenter",
                table: "BloodTransferCenters",
                sql: "1=(SELECT CASE WHEN COUNT(*) <= 1 THEN 1 ELSE 0 END FROM \"BloodTransferCenters\")");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SingleBloodTransferCenter",
                table: "BloodTransferCenters");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BloodBags");
        }
    }
}
