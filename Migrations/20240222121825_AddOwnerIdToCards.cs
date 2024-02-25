using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mtcg.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Cards",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Cards");
        }
    }
}
