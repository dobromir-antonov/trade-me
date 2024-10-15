using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Orders.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.CreateTable(
                name: "outbox_integration_events",
                schema: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "JSONB", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_integration_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tickers",
                schema: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    last_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticker_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_tickers_ticker_id",
                        column: x => x.ticker_id,
                        principalSchema: "orders",
                        principalTable: "tickers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_ticker_id",
                schema: "orders",
                table: "orders",
                column: "ticker_id");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_integration_events_created_on_utc_processed_on_utc",
                schema: "orders",
                table: "outbox_integration_events",
                columns: new[] { "created_on_utc", "processed_on_utc" })
                .Annotation("Npgsql:IndexInclude", new[] { "id", "type", "content" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "outbox_integration_events",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "tickers",
                schema: "orders");
        }
    }
}
