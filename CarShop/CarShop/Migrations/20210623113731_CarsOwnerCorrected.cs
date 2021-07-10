using Microsoft.EntityFrameworkCore.Migrations;

namespace CarShop.Migrations
{
    public partial class CarsOwnerCorrected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cas_Users_OwnnerId",
                table: "Cas");

            migrationBuilder.DropIndex(
                name: "IX_Cas_OwnnerId",
                table: "Cas");

            migrationBuilder.DropColumn(
                name: "OwnnerId",
                table: "Cas");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Cas",
                type: "nvarchar(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Cas_OwnerId",
                table: "Cas",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cas_Users_OwnerId",
                table: "Cas",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cas_Users_OwnerId",
                table: "Cas");

            migrationBuilder.DropIndex(
                name: "IX_Cas_OwnerId",
                table: "Cas");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Cas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)");

            migrationBuilder.AddColumn<string>(
                name: "OwnnerId",
                table: "Cas",
                type: "nvarchar(40)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cas_OwnnerId",
                table: "Cas",
                column: "OwnnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cas_Users_OwnnerId",
                table: "Cas",
                column: "OwnnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
