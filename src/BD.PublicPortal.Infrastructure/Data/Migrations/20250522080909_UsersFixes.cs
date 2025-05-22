using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.PublicPortal.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UsersFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CorrelationId",
                table: "AspNetUsers",
                newName: "DonorCorrelationId");

            migrationBuilder.AlterColumn<bool>(
                name: "DonorWantToStayAnonymous",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "DonorExcludeFromPublicPortal",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DonorCorrelationId",
                table: "AspNetUsers",
                newName: "CorrelationId");

            migrationBuilder.AlterColumn<bool>(
                name: "DonorWantToStayAnonymous",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "DonorExcludeFromPublicPortal",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
