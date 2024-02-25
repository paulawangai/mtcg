using Microsoft.EntityFrameworkCore.Migrations;

namespace mtcg.Migrations
{
    public partial class UpdateCardTypeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Cards",
                nullable: true,  // Make the "Type" column nullable
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Cards",
                nullable: false,  // Restore the "Type" column to not nullable
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
