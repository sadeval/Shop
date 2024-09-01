using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logir.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 9, 1, 19, 2, 50, 318, DateTimeKind.Local).AddTicks(1906));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 9, 1, 18, 54, 53, 740, DateTimeKind.Local).AddTicks(9909));
        }
    }
}
