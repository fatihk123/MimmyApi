using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProjeKampi.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutDescrption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descrption",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descrption",
                table: "Abouts");
        }
    }
}
