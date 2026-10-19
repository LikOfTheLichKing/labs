using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Infrastructure.Migrations
{
    public partial class Migration_v2_ContractOldPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Удаляем старое поле (выполняется только после перехода всего кода на новое)
            migrationBuilder.DropColumn(
                name: "OldPriceAmount",
                table: "Ads");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OldPriceAmount",
                table: "Ads",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}