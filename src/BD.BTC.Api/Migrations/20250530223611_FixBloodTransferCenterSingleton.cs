using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSTS_Back.Migrations
{
    /// <inheritdoc />
    public partial class FixBloodTransferCenterSingleton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BloodTransferCenters_Email",
                table: "BloodTransferCenters");

            migrationBuilder.DropIndex(
                name: "IX_BloodTransferCenters_IsPrimary",
                table: "BloodTransferCenters");

            migrationBuilder.DropIndex(
                name: "IX_BloodTransferCenters_Name",
                table: "BloodTransferCenters");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "BloodTransferCenters");

            migrationBuilder.AddColumn<bool>(
                name: "IsSingleton",
                table: "BloodTransferCenters",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_BloodTransferCenters_IsSingleton",
                table: "BloodTransferCenters",
                column: "IsSingleton",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BloodTransferCenters_IsSingleton",
                table: "BloodTransferCenters");

            migrationBuilder.DropColumn(
                name: "IsSingleton",
                table: "BloodTransferCenters");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "BloodTransferCenters",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BloodTransferCenters_Email",
                table: "BloodTransferCenters",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_BloodTransferCenters_IsPrimary",
                table: "BloodTransferCenters",
                column: "IsPrimary",
                unique: true,
                filter: "\"IsPrimary\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_BloodTransferCenters_Name",
                table: "BloodTransferCenters",
                column: "Name",
                unique: true);
        }
    }
}
