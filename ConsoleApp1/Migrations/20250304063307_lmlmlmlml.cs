using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApp1.Migrations
{
    /// <inheritdoc />
    public partial class lmlmlmlml : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuPageDatas");

            migrationBuilder.CreateTable(
                name: "MenuPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuPageApis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuPageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPageApis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuPageApis_MenuPages_MenuPageId",
                        column: x => x.MenuPageId,
                        principalTable: "MenuPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuPageRedirects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuPageId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_MenuPageApis_MenuPageId",
                table: "MenuPageApis",
                column: "MenuPageId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPageRedirects_MenuPageId",
                table: "MenuPageRedirects",
                column: "MenuPageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuPageApis");

            migrationBuilder.DropTable(
                name: "MenuPageRedirects");

            migrationBuilder.DropTable(
                name: "MenuPages");

            migrationBuilder.CreateTable(
                name: "MenuPageDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedirectUrls = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPageDatas", x => x.Id);
                });
        }
    }
}
