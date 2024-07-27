using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Microservice.Order.Api.Migrations
{
    /// <inheritdoc />
    public partial class createtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "OrderNumberCounter");

            migrationBuilder.CreateTable(
                name: "MSOS_OrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSOS_OrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MSOS_ProductType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSOS_ProductType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MSOS_Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR OrderNumberCounter"),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(19,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSOS_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSOS_Order_MSOS_OrderStatus_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "MSOS_OrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MSOS_OrderItem",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(19,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSOS_OrderItem", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_MSOS_OrderItem_MSOS_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "MSOS_Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MSOS_OrderItem_MSOS_ProductType_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "MSOS_ProductType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MSOS_OrderStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "Created" },
                    { 2, "Paid" },
                    { 3, "Dispatched" },
                    { 4, "Recieved" },
                    { 5, "Completed" }
                });

            migrationBuilder.InsertData(
                table: "MSOS_ProductType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Book" },
                    { 2, "Music" }
                });

            migrationBuilder.InsertData(
                table: "MSOS_Order",
                columns: new[] { "Id", "Created", "CustomerAddressId", "CustomerId", "LastUpdated", "OrderNumber", "OrderStatusId", "Total" },
                values: new object[,]
                {
                    { new Guid("07a2ab81-7651-484f-945e-08074e7662bc"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(871), new Guid("0a1cbffa-967f-4338-a2c1-e80238d61a16"), new Guid("160429ab-7d0b-464e-8042-cec3218c014c"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(873), 8, 5, 12.99m },
                    { new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(853), new Guid("97d31501-4008-4b1a-9aeb-71d4cea31059"), new Guid("39c2080b-18ca-4974-8937-f9d758b89bac"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(854), 5, 1, 24.98m },
                    { new Guid("34335ca4-ebcd-4a2e-b54a-af9feb09535d"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(860), new Guid("14e016ec-3935-431f-88a3-17b55ad99198"), new Guid("453b920e-e8b0-4c5e-bd44-77c4cd75771d"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(862), 6, 2, 19.98m },
                    { new Guid("54d54c4e-d774-4b01-af87-0d0b94510767"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(866), new Guid("870062c2-09c1-4fd4-8066-7d149b5cc86c"), new Guid("4de2c877-5cdb-4153-9e49-4a8f77d910e9"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(867), 7, 5, 28.78m },
                    { new Guid("69e3878e-8293-4e81-8791-31328e2a3907"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(883), new Guid("2bc333d2-8f8c-46bd-b454-b0735b662bea"), new Guid("82c5ba18-d049-49a0-83aa-a2a9840b08ad"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(884), 10, 5, 10.00m },
                    { new Guid("6f3e950c-5502-491f-911d-02e112318705"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(838), new Guid("2bc333d2-8f8c-46bd-b454-b0735b662bea"), new Guid("82c5ba18-d049-49a0-83aa-a2a9840b08ad"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(839), 3, 3, 2.50m },
                    { new Guid("7b5673fa-166e-417e-a0bf-d2c7d72b6ab3"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(781), new Guid("eeed04b3-538b-4677-887b-77f0803958a6"), new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(783), 2, 2, 19.98m },
                    { new Guid("9e93a116-a143-4865-8a84-b000d0df09c7"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(877), new Guid("25ed9fe9-4c1d-414e-b6b3-bcf725dce00b"), new Guid("3ea4739c-a8dc-4c59-b3ba-c6104024c24e"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(878), 9, 5, 59.96m },
                    { new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(726), new Guid("9a11e147-f416-4063-a5ed-94bae3bce423"), new Guid("bb472ce8-edfd-4b90-8f11-40f3eeed778b"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(770), 1, 1, 25.99m },
                    { new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(843), new Guid("9a11e147-f416-4063-a5ed-94bae3bce423"), new Guid("bb472ce8-edfd-4b90-8f11-40f3eeed778b"), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(845), 4, 4, 51.47m }
                });

            migrationBuilder.InsertData(
                table: "MSOS_OrderItem",
                columns: new[] { "OrderId", "ProductId", "Name", "ProductTypeId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { new Guid("07a2ab81-7651-484f-945e-08074e7662bc"), new Guid("285c81bc-f257-4ffb-b6ce-7ab5fa9e5c81"), "Skandar and the Chaos Trials", 1, 1, 12.99m },
                    { new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), "Infinity Son", 1, 2, 7.50m },
                    { new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"), "Infinity Kings", 1, 2, 9.99m },
                    { new Guid("34335ca4-ebcd-4a2e-b54a-af9feb09535d"), new Guid("37544155-da95-49e8-b7fe-3c937eb1de98"), "Wild Love", 1, 2, 9.99m },
                    { new Guid("54d54c4e-d774-4b01-af87-0d0b94510767"), new Guid("6b85f863-7991-4f93-bf86-8c756fdeac87"), "Fall of Civilizations: Stories of Greatness and Decline", 1, 2, 14.39m },
                    { new Guid("69e3878e-8293-4e81-8791-31328e2a3907"), new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), "Infinity Son", 1, 1, 7.50m },
                    { new Guid("69e3878e-8293-4e81-8791-31328e2a3907"), new Guid("f3fcab1f-1c11-47f5-9e11-7868a88408e6"), "Thunderhead", 1, 1, 2.50m },
                    { new Guid("6f3e950c-5502-491f-911d-02e112318705"), new Guid("f3fcab1f-1c11-47f5-9e11-7868a88408e6"), "Thunderhead", 1, 1, 2.50m },
                    { new Guid("7b5673fa-166e-417e-a0bf-d2c7d72b6ab3"), new Guid("37544155-da95-49e8-b7fe-3c937eb1de98"), "Wild Love", 1, 2, 9.99m },
                    { new Guid("9e93a116-a143-4865-8a84-b000d0df09c7"), new Guid("01f54aa7-c51a-4b92-a72b-68e0965bf246"), "Funny Story", 1, 1, 11.99m },
                    { new Guid("9e93a116-a143-4865-8a84-b000d0df09c7"), new Guid("ecf65c56-5670-473b-9f20-fb0b191c2f0f"), "Saltblood", 1, 3, 15.99m },
                    { new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), "Infinity Son", 1, 1, 7.50m },
                    { new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"), "Infinity Kings", 1, 1, 9.99m },
                    { new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), new Guid("6131ce7e-fb11-4608-a3d3-f01caee2c465"), "Infinity Reaper", 1, 1, 8.50m },
                    { new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"), new Guid("23608dce-2142-4d2b-b909-948316b5efaf"), "Scythe", 1, 1, 3.50m },
                    { new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"), new Guid("ecf65c56-5670-473b-9f20-fb0b191c2f0f"), "Saltblood", 1, 3, 15.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MSOS_Order_OrderStatusId",
                table: "MSOS_Order",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MSOS_OrderItem_ProductTypeId",
                table: "MSOS_OrderItem",
                column: "ProductTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MSOS_OrderItem");

            migrationBuilder.DropTable(
                name: "MSOS_Order");

            migrationBuilder.DropTable(
                name: "MSOS_ProductType");

            migrationBuilder.DropTable(
                name: "MSOS_OrderStatus");

            migrationBuilder.DropSequence(
                name: "OrderNumberCounter");
        }
    }
}
