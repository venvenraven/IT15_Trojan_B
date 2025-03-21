using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT15_Trojan_B.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserRoleToSecurityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "SecurityLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "SecurityLogs");
        }
    }
}
