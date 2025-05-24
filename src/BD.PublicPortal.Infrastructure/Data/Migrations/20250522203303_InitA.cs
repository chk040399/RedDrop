using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.PublicPortal.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodInventories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodInventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BloodTansfusionCenterId = table.Column<Guid>(type: "uuid", nullable: false),
                    BloodDonationType = table.Column<int>(type: "integer", nullable: false),
                    BloodGroup = table.Column<int>(type: "integer", nullable: false),
                    MaxQty = table.Column<int>(type: "integer", nullable: true),
                    MinQty = table.Column<int>(type: "integer", nullable: true),
                    TotalQty = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodInventories_BloodTansfusionCenters_BloodTansfusionCent~",
                        column: x => x.BloodTansfusionCenterId,
                        principalTable: "BloodTansfusionCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodInventories_BloodTansfusionCenterId",
                table: "BloodInventories",
                column: "BloodTansfusionCenterId");
        }
    }
}
