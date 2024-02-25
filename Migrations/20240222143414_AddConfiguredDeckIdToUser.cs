using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mtcg.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguredDeckIdToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConfiguredDeckId",
                table: "Users",
                type: "uuid",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "ConfiguredDeckId",
               table: "Users");
        }
    }
}
