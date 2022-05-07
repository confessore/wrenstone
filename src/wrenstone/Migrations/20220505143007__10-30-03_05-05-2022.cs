using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wrenstone.Migrations
{
    public partial class _103003_05052022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
