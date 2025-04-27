using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MoorescnrWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Guns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ManufacturedYear = table.Column<short>(type: "smallint", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false),
                    SalePrice = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guns", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false),
                    GunId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, comment: "The order which the pictures are displayed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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
