using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kash.Elector.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElectoralLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Party = table.Column<string>(maxLength: 128, nullable: true),
                    DistrictId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectoralLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ElectionId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Seats = table.Column<int>(nullable: false),
                    ElectoralListId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Districts_ElectoralLists_ElectoralListId",
                        column: x => x.ElectoralListId,
                        principalTable: "ElectoralLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Electors",
                columns: table => new
                {
                    CredentialId = table.Column<string>(maxLength: 36, nullable: false),
                    DistrictId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Electors", x => x.CredentialId);
                    table.ForeignKey(
                        name: "FK_Electors_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ElectionId",
                table: "Districts",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ElectoralListId",
                table: "Districts",
                column: "ElectoralListId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectoralLists_DistrictId",
                table: "ElectoralLists",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Electors_DistrictId",
                table: "Electors",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectoralLists_Districts_DistrictId",
                table: "ElectoralLists",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Elections_ElectionId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_ElectoralLists_ElectoralListId",
                table: "Districts");

            migrationBuilder.DropTable(
                name: "Electors");

            migrationBuilder.DropTable(
                name: "Elections");

            migrationBuilder.DropTable(
                name: "ElectoralLists");

            migrationBuilder.DropTable(
                name: "Districts");
        }
    }
}
