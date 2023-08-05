using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoticiarioAPI.Migrations
{
    public partial class TesteJWT2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Usuarios");
        }
    }
}
