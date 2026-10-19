using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Infrastructure.Migrations
{
    public partial class Migration_v1_ExpandAdPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Добавляем новое поле со скидкой (nullable: true)
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountedAmount",
                table: "Ads",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountedAmount",
                table: "Ads");
        }
    }
}