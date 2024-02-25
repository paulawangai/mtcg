using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mtcg.Migrations
{
    /// <inheritdoc />
    public partial class AddCoinsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
            name: "Coins",
            table: "Users",
            nullable: false,
            defaultValue: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "Coins",
            table: "Users");
        }
    }
}
