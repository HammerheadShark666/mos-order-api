using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Microservice.Order.Api.Migrations
{
    /// <inheritdoc />
    public partial class createtabledefaultdata : Migration
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
                    AddressSurname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressForename = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(19,2)", nullable: true)
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
                columns: new[] { "Id", "AddressForename", "AddressSurname", "Created", "CustomerAddressId", "CustomerId", "LastUpdated", "OrderNumber", "OrderStatusId", "Total" },
                values: new object[,]
                {
                    { new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), "Intergration_Test2", "Intergration_Test2", new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(795), new Guid("b88ef4ce-739f-4c1b-b6d6-9d0727515de8"), new Guid("929eaf82-e4fd-4efe-9cae-ce4d7e32d159"), new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(797), 5, 1, 24.98m },
                    { new Guid("6f3e950c-5502-491f-911d-02e112318705"), "Intergration_Test", "Intergration_Test", new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(782), new Guid("724cbd34-3dff-4e2a-a413-48825f1ab3b9"), new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(784), 3, 3, 2.50m },
                    { new Guid("7b5673fa-166e-417e-a0bf-d2c7d72b6ab3"), "Intergration_Test", "Intergration_Test", new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(776), new Guid("724cbd34-3dff-4e2a-a413-48825f1ab3b9"), new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(778), 2, 2, 19.98m },
                    { new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"), "Intergration_Test", "Intergration_Test", new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(724), new Guid("724cbd34-3dff-4e2a-a413-48825f1ab3b9"), new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(767), 1, 1, 25.99m },
                    { new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"), "Intergration_Test2", "Intergration_Test2", new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(789), new Guid("b88ef4ce-739f-4c1b-b6d6-9d0727515de8"), new Guid("929eaf82-e4fd-4efe-9cae-ce4d7e32d159"), new DateTime(2024, 7, 27, 14, 25, 35, 752, DateTimeKind.Local).AddTicks(790), 4, 4, 51.47m }
                });

            migrationBuilder.InsertData(
                table: "MSOS_OrderItem",
                columns: new[] { "OrderId", "ProductId", "Name", "ProductTypeId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new Guid("07c06c3f-0897-44b6-ae05-a70540e73a12"), "Infinity Son", 1, 2, 7.50m },
                    { new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"), new Guid("29a75938-ce2d-473b-b7fe-2903fe97fd6e"), "Infinity Kings", 1, 2, 9.99m },
                    { new Guid("6f3e950c-5502-491f-911d-02e112318705"), new Guid("f3fcab1f-1c11-47f5-9e11-7868a88408e6"), "Thunderhead", 1, 1, 2.50m },
                    { new Guid("7b5673fa-166e-417e-a0bf-d2c7d72b6ab3"), new Guid("37544155-da95-49e8-b7fe-3c937eb1de98"), "Wild Love", 1, 2, 9.99m },
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
