using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSTS_Back.Migrations
{
    /// <inheritdoc />
    public partial class newMIGRATIONS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FulfillmentDate",
                table: "Pledges");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PledgeDate",
                table: "Pledges",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PledgeDate",
                table: "Pledges",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FulfillmentDate",
                table: "Pledges",
                type: "date",
                nullable: true);
        }
    }
}
