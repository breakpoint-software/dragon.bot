using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class UsdAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UsdAmount",
                table: "Order",
                type: "decimal(16,2)",
                nullable: false,
                defaultValue: 0m);
            migrationBuilder.Sql("UPDATE `Order` SET UsdAmount = Price * Quantity;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsdAmount",
                table: "Order");
        }
    }
}
