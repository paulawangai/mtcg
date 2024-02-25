using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mtcg.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNumberOfCardsFromPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "NumberOfCards",
            table: "Packages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
            name: "NumberOfCards",
            table: "Packages",
            type: "int",
            nullable: false,
            defaultValue: 0);
        }
    }
}
