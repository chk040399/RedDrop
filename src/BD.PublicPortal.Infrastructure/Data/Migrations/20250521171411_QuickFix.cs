using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.PublicPortal.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuickFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Wilayas",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Communes",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "EvolutionStatus",
                table: "BloodDonationRequests",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Wilayas",
                newName: "Nom");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Communes",
                newName: "Nom");

            migrationBuilder.AlterColumn<int>(
                name: "EvolutionStatus",
                table: "BloodDonationRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
