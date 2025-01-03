using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eticket.Migrations
{
    /// <inheritdoc />
    public partial class addblocked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ISBlocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddUserToRoleVMId",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AddUserToRoleVM",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddUserToRoleVM", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_AddUserToRoleVMId",
                table: "AspNetRoles",
                column: "AddUserToRoleVMId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AddUserToRoleVM_AddUserToRoleVMId",
                table: "AspNetRoles",
                column: "AddUserToRoleVMId",
                principalTable: "AddUserToRoleVM",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AddUserToRoleVM_AddUserToRoleVMId",
                table: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AddUserToRoleVM");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_AddUserToRoleVMId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "ISBlocked",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AddUserToRoleVMId",
                table: "AspNetRoles");
        }
    }
}
