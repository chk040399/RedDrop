using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.PublicPortal.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DonnorCorrelationID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CorrelationId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "AspNetUsers");
        }
    }
}
