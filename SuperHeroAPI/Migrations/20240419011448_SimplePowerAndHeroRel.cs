using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperHeroAPI.Migrations
{
    /// <inheritdoc />
    public partial class SimplePowerAndHeroRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PowerSuperHero",
                columns: table => new
                {
                    SuperHeroesId = table.Column<int>(type: "int", nullable: false),
                    SuperHeroesId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerSuperHero", x => new { x.SuperHeroesId, x.SuperHeroesId1 });
                    table.ForeignKey(
                        name: "FK_PowerSuperHero_Powers_SuperHeroesId",
                        column: x => x.SuperHeroesId,
                        principalTable: "Powers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PowerSuperHero_SuperHeroes_SuperHeroesId1",
                        column: x => x.SuperHeroesId1,
                        principalTable: "SuperHeroes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerSuperHero_SuperHeroesId1",
                table: "PowerSuperHero",
                column: "SuperHeroesId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerSuperHero");
        }
    }
}
