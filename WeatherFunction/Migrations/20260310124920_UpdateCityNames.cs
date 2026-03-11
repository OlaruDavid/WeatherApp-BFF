using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherFunction.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCityNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 16,
                column: "CityName",
                value: "Bucharest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 16,
                column: "CityName",
                value: "Bucuresti");
        }
    }
}
