using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagement.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_Users",
                columns: table => new
                {
                    n_UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    s_UserCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    s_UserName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    s_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Users", x => x.n_UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_Users");
        }
    }
}
