using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLaundry.Migrations
{
    /// <inheritdoc />
    public partial class FixUserEmailForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Users_UserEmail",
                table: "Machines");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Machines",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_UserEmail",
                table: "Machines",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "UserEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Users_UserEmail",
                table: "Machines");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Machines",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Users_UserEmail",
                table: "Machines",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "UserEmail",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
