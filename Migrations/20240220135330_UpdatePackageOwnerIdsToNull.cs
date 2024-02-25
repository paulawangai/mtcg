using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mtcg.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePackageOwnerIdsToNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Packages\" SET \"OwnerId\" = NULL");

        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Implement code to revert changes if needed
        }

    }
}

