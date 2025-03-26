using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SimpleApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COM_CUSTOMER",
                columns: table => new
                {
                    COM_CUSTOMER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CUSTOMER_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COM_CUSTOMER", x => x.COM_CUSTOMER_ID);
                });

            migrationBuilder.CreateTable(
                name: "SO_ORDER",
                columns: table => new
                {
                    SO_ORDER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORDER_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ORDER_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    COM_CUSTOMER_ID = table.Column<int>(type: "int", nullable: false),
                    ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_ORDER", x => x.SO_ORDER_ID);
                    table.ForeignKey(
                        name: "FK_SO_ORDER_COM_CUSTOMER_COM_CUSTOMER_ID",
                        column: x => x.COM_CUSTOMER_ID,
                        principalTable: "COM_CUSTOMER",
                        principalColumn: "COM_CUSTOMER_ID");
                });

            migrationBuilder.CreateTable(
                name: "SO_ITEM",
                columns: table => new
                {
                    SO_ITEM_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SO_ORDER_ID = table.Column<int>(type: "int", nullable: false),
                    ITEM_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QUANTITY = table.Column<int>(type: "int", nullable: false),
                    PRICE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_ITEM", x => x.SO_ITEM_ID);
                    table.ForeignKey(
                        name: "FK_SO_ITEM_SO_ORDER_SO_ORDER_ID",
                        column: x => x.SO_ORDER_ID,
                        principalTable: "SO_ORDER",
                        principalColumn: "SO_ORDER_ID");
                });

            migrationBuilder.InsertData(
                table: "COM_CUSTOMER",
                columns: new[] { "COM_CUSTOMER_ID", "CUSTOMER_NAME" },
                values: new object[,]
                {
                    { 1, "Alpha" },
                    { 2, "Beta" },
                    { 3, "Gamma" }
                });

            migrationBuilder.InsertData(
                table: "SO_ORDER",
                columns: new[] { "SO_ORDER_ID", "ADDRESS", "COM_CUSTOMER_ID", "ORDER_DATE", "ORDER_NO" },
                values: new object[,]
                {
                    { 1, "Jl Nangka", 1, new DateTime(2025, 3, 27, 5, 43, 46, 57, DateTimeKind.Local).AddTicks(5868), "Order-1001" },
                    { 2, "Jl Kampung Rambutan", 1, new DateTime(2025, 3, 27, 5, 43, 46, 57, DateTimeKind.Local).AddTicks(5877), "Order-1002" },
                    { 3, "Jl Pabuaran", 2, new DateTime(2025, 3, 27, 5, 43, 46, 57, DateTimeKind.Local).AddTicks(5879), "Order-1003" },
                    { 4, "Jl Kepandaian", 2, new DateTime(2025, 3, 27, 5, 43, 46, 57, DateTimeKind.Local).AddTicks(5880), "Order-1004" },
                    { 5, "Jl Pasuruan", 3, new DateTime(2025, 3, 27, 5, 43, 46, 57, DateTimeKind.Local).AddTicks(5881), "Order-1005" },
                    { 6, "Jl Gajah", 3, new DateTime(2025, 3, 27, 5, 43, 46, 57, DateTimeKind.Local).AddTicks(5883), "Order-1006" }
                });

            migrationBuilder.InsertData(
                table: "SO_ITEM",
                columns: new[] { "SO_ITEM_ID", "ITEM_NAME", "PRICE", "QUANTITY", "SO_ORDER_ID" },
                values: new object[,]
                {
                    { 1, "Kursi", 50000, 10, 1 },
                    { 2, "Meja", 100000, 5, 1 },
                    { 3, "Tabung Gas", 25000, 8, 2 },
                    { 4, "Kompor Gas", 200000, 3, 2 },
                    { 5, "Jendela", 500000, 3, 3 },
                    { 6, "Pintu", 100000, 10, 4 },
                    { 7, "Monitor", 1000000, 2, 5 },
                    { 8, "AC", 900000, 5, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SO_ITEM_SO_ORDER_ID",
                table: "SO_ITEM",
                column: "SO_ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SO_ORDER_COM_CUSTOMER_ID",
                table: "SO_ORDER",
                column: "COM_CUSTOMER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SO_ITEM");

            migrationBuilder.DropTable(
                name: "SO_ORDER");

            migrationBuilder.DropTable(
                name: "COM_CUSTOMER");
        }
    }
}
