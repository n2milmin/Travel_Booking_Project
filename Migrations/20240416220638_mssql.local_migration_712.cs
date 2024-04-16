using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GBC_Travel_Group_136.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_712 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "Flights",
                keyColumn: "FlightId",
                keyValue: 1,
                columns: new[] { "Arrival", "Departure" },
                values: new object[] { new DateTime(2024, 4, 16, 18, 6, 37, 573, DateTimeKind.Local).AddTicks(2204), new DateTime(2024, 4, 16, 18, 6, 37, 573, DateTimeKind.Local).AddTicks(2146) });

            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "Flights",
                keyColumn: "FlightId",
                keyValue: 2,
                columns: new[] { "Arrival", "Departure" },
                values: new object[] { new DateTime(2024, 4, 16, 18, 6, 37, 573, DateTimeKind.Local).AddTicks(2224), new DateTime(2024, 4, 16, 18, 6, 37, 573, DateTimeKind.Local).AddTicks(2223) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "Flights",
                keyColumn: "FlightId",
                keyValue: 1,
                columns: new[] { "Arrival", "Departure" },
                values: new object[] { new DateTime(2024, 4, 16, 15, 37, 42, 36, DateTimeKind.Local).AddTicks(6003), new DateTime(2024, 4, 16, 15, 37, 42, 36, DateTimeKind.Local).AddTicks(5956) });

            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "Flights",
                keyColumn: "FlightId",
                keyValue: 2,
                columns: new[] { "Arrival", "Departure" },
                values: new object[] { new DateTime(2024, 4, 16, 15, 37, 42, 36, DateTimeKind.Local).AddTicks(6020), new DateTime(2024, 4, 16, 15, 37, 42, 36, DateTimeKind.Local).AddTicks(6019) });
        }
    }
}
