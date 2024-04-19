using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperHeroAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSimpleJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PowerSuperHero",
                columns: table => new
                {
                    PowersId = table.Column<int>(type: "int", nullable: false),
                    SuperHeroesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerSuperHero", x => new { x.PowersId, x.SuperHeroesId });
                    table.ForeignKey(
                        name: "FK_PowerSuperHero_Powers_PowersId",
                        column: x => x.PowersId,
                        principalTable: "Powers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PowerSuperHero_SuperHeroes_SuperHeroesId",
                        column: x => x.SuperHeroesId,
                        principalTable: "SuperHeroes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerSuperHero_SuperHeroesId",
                table: "PowerSuperHero",
                column: "SuperHeroesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerSuperHero");
        }
    }
}
