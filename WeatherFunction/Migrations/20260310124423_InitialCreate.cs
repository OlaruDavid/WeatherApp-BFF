using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WeatherFunction.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityName = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    IsMainCity = table.Column<bool>(type: "boolean", nullable: false),
                    NearestMainCityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Cities_NearestMainCityId",
                        column: x => x.NearestMainCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SearchHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    CityName = table.Column<string>(type: "text", nullable: false),
                    SearchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchHistories_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherCurrents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    WeatherData = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherCurrents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherCurrents_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    WeatherData = table.Column<string>(type: "text", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherHistories_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityName", "IsMainCity", "Latitude", "Longitude", "NearestMainCityId" },
                values: new object[,]
                {
                    { 1, "Iasi", true, 47.159999999999997, 27.579999999999998, null },
                    { 2, "Suceava", true, 47.630000000000003, 26.25, null },
                    { 3, "Bacau", true, 46.57, 26.91, null },
                    { 4, "Vaslui", true, 46.640000000000001, 27.73, null },
                    { 5, "Roman", true, 46.920000000000002, 26.93, null },
                    { 6, "Focsani", true, 45.700000000000003, 27.18, null },
                    { 7, "Galati", true, 45.43, 28.030000000000001, null },
                    { 8, "Barlad", true, 46.229999999999997, 27.670000000000002, null },
                    { 9, "Constanta", true, 44.18, 28.649999999999999, null },
                    { 10, "Tulcea", true, 45.18, 28.800000000000001, null },
                    { 11, "Braila", true, 45.270000000000003, 27.960000000000001, null },
                    { 12, "Buzau", true, 45.149999999999999, 26.829999999999998, null },
                    { 13, "Medgidia", true, 44.25, 28.27, null },
                    { 14, "Mangalia", true, 43.82, 28.579999999999998, null },
                    { 15, "Slobozia", true, 44.560000000000002, 27.370000000000001, null },
                    { 16, "Bucuresti", true, 44.43, 26.100000000000001, null },
                    { 17, "Ploiesti", true, 44.939999999999998, 26.02, null },
                    { 18, "Targoviste", true, 44.93, 25.449999999999999, null },
                    { 19, "Alexandria", true, 43.969999999999999, 25.329999999999998, null },
                    { 20, "Calarasi", true, 44.200000000000003, 27.329999999999998, null },
                    { 21, "Craiova", true, 44.32, 23.800000000000001, null },
                    { 22, "Pitesti", true, 44.850000000000001, 24.870000000000001, null },
                    { 23, "Ramnicu Valcea", true, 45.100000000000001, 24.370000000000001, null },
                    { 24, "Targu Jiu", true, 45.039999999999999, 23.280000000000001, null },
                    { 25, "Drobeta Turnu Severin", true, 44.630000000000003, 22.66, null },
                    { 26, "Timisoara", true, 45.75, 21.23, null },
                    { 27, "Arad", true, 46.18, 21.309999999999999, null },
                    { 28, "Oradea", true, 47.07, 21.920000000000002, null },
                    { 29, "Deva", true, 45.880000000000003, 22.91, null },
                    { 30, "Cluj", true, 46.770000000000003, 23.59, null },
                    { 31, "Baia Mare", true, 47.659999999999997, 23.579999999999998, null },
                    { 32, "Satu Mare", true, 47.789999999999999, 22.879999999999999, null },
                    { 33, "Zalau", true, 47.189999999999998, 23.059999999999999, null },
                    { 34, "Bistrita", true, 47.130000000000003, 24.5, null },
                    { 35, "Brasov", true, 45.649999999999999, 25.609999999999999, null },
                    { 36, "Sibiu", true, 45.799999999999997, 24.149999999999999, null },
                    { 37, "Targu Mures", true, 46.539999999999999, 24.559999999999999, null },
                    { 38, "Alba Iulia", true, 46.07, 23.579999999999998, null },
                    { 39, "Sfantu Gheorghe", true, 45.859999999999999, 25.789999999999999, null },
                    { 40, "Miercurea Ciuc", true, 46.359999999999999, 25.800000000000001, null },
                    { 41, "Sinaia", true, 45.350000000000001, 25.550000000000001, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CityName",
                table: "Cities",
                column: "CityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_NearestMainCityId",
                table: "Cities",
                column: "NearestMainCityId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistories_CityId",
                table: "SearchHistories",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherCurrents_CityId",
                table: "WeatherCurrents",
                column: "CityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherHistories_CityId",
                table: "WeatherHistories",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchHistories");

            migrationBuilder.DropTable(
                name: "WeatherCurrents");

            migrationBuilder.DropTable(
                name: "WeatherHistories");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
