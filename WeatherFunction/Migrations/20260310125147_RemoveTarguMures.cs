using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherFunction.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTarguMures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 37);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityName", "IsMainCity", "Latitude", "Longitude", "NearestMainCityId" },
                values: new object[] { 37, "Targu Mures", true, 46.539999999999999, 24.559999999999999, null });
        }
    }
}
