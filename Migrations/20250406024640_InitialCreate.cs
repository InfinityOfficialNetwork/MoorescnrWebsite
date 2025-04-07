using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoorescnrWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ManufacturedYear = table.Column<short>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    SalePrice = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false),
                    GunId = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false, comment: "The order which the pictures are displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_guns_description",
                table: "Guns",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "idx_guns_id",
                table: "Guns",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "idx_guns_name",
                table: "Guns",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "idx_guns_price",
                table: "Guns",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "idx_guns_saleprice",
                table: "Guns",
                column: "SalePrice");

            migrationBuilder.CreateIndex(
                name: "idx_images_gunid",
                table: "Images",
                column: "GunId");

            migrationBuilder.CreateIndex(
                name: "idx_images_id",
                table: "Images",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guns");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
