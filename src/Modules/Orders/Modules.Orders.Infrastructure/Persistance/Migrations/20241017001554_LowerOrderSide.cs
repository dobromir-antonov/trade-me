using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Orders.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class LowerOrderSide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Side",
                schema: "orders",
                table: "orders",
                newName: "side");

            migrationBuilder.AlterColumn<int>(
                name: "side",
                schema: "orders",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "side",
                schema: "orders",
                table: "orders",
                newName: "Side");

            migrationBuilder.AlterColumn<int>(
                name: "Side",
                schema: "orders",
                table: "orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);
        }
    }
}
