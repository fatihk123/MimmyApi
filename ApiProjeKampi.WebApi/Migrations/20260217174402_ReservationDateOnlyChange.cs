using Microsoft.EntityFrameworkCore.Migrations;

using System;

#nullable disable

namespace ApiProjeKampi.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ReservationDateOnlyChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReservationDate",
                table: "Reservations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReservationDate",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
