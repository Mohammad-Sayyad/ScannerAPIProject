using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApp1.Migrations
{
    /// <inheritdoc />
    public partial class mnmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuPageRedirects");

            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "MenuPageApis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "MenuPageApis");

            migrationBuilder.CreateTable(
                name: "MenuPageRedirects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuPageId = table.Column<int>(type: "int", nullable: false),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPageRedirects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuPageRedirects_MenuPages_MenuPageId",
                        column: x => x.MenuPageId,
                        principalTable: "MenuPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuPageRedirects_MenuPageId",
                table: "MenuPageRedirects",
                column: "MenuPageId");
        }
    }
}
