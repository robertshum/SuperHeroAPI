using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperHeroAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameSuperHeroesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuperHeroPowers_SuperHeros_SuperHeroId",
                table: "SuperHeroPowers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuperHeros",
                table: "SuperHeros");

            migrationBuilder.RenameTable(
                name: "SuperHeros",
                newName: "SuperHeroes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuperHeroes",
                table: "SuperHeroes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuperHeroPowers_SuperHeroes_SuperHeroId",
                table: "SuperHeroPowers",
                column: "SuperHeroId",
                principalTable: "SuperHeroes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuperHeroPowers_SuperHeroes_SuperHeroId",
                table: "SuperHeroPowers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuperHeroes",
                table: "SuperHeroes");

            migrationBuilder.RenameTable(
                name: "SuperHeroes",
                newName: "SuperHeros");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuperHeros",
                table: "SuperHeros",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuperHeroPowers_SuperHeros_SuperHeroId",
                table: "SuperHeroPowers",
                column: "SuperHeroId",
                principalTable: "SuperHeros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
