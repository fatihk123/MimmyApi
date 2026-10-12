using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProjeKampi.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class FeatureImageUrlRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "İmageUrl",
                table: "Features",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Features",
                newName: "İmageUrl");
        }
    }
}
