using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping_Cart_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class model3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
