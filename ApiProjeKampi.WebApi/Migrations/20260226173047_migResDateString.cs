using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProjeKampi.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class migResDateString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReservationTime",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "ReservationTime",
                table: "Reservations",
                type: "time",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
