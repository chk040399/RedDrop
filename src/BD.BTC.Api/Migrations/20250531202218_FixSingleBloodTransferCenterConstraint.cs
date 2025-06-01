using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSTS_Back.Migrations
{
    /// <inheritdoc />
    public partial class FixSingleBloodTransferCenterConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SingleBloodTransferCenter",
                table: "BloodTransferCenters");

            migrationBuilder.AddColumn<int>(
                name: "SingletonCheck",
                table: "BloodTransferCenters",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_BloodTransferCenters_SingletonCheck",
                table: "BloodTransferCenters",
                column: "SingletonCheck",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BloodTransferCenters_SingletonCheck",
                table: "BloodTransferCenters");

            migrationBuilder.DropColumn(
                name: "SingletonCheck",
                table: "BloodTransferCenters");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SingleBloodTransferCenter",
                table: "BloodTransferCenters",
                sql: "1=(SELECT CASE WHEN COUNT(*) <= 1 THEN 1 ELSE 0 END FROM \"BloodTransferCenters\")");
        }
    }
}
