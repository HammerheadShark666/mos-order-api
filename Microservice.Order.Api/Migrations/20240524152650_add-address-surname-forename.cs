using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservice.Order.Api.Migrations
{
    /// <inheritdoc />
    public partial class addaddresssurnameforename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "MSOS_OrderItem",
                type: "decimal(19,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MSOS_OrderItem",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "AddressForename",
                table: "MSOS_Order",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressSurname",
                table: "MSOS_Order",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("07a2ab81-7651-484f-945e-08074e7662bc"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "Ellen", "Frown", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2717), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2719) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "Burt", "Mortenson", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2697), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2698) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("34335ca4-ebcd-4a2e-b54a-af9feb09535d"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "Grace", "Keltso", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2704), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2705) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("54d54c4e-d774-4b01-af87-0d0b94510767"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "Rachel", "Arbring", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2711), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2712) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("69e3878e-8293-4e81-8791-31328e2a3907"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "Sano", "Yot", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2730), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2732) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("6f3e950c-5502-491f-911d-02e112318705"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "Michael", "Willow", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2684), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2686) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("7b5673fa-166e-417e-a0bf-d2c7d72b6ab3"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "Kelly", "Johnson", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2677), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2679) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("9e93a116-a143-4865-8a84-b000d0df09c7"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "William", "Gordonson", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2724), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2725) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "John", "West", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2619), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2665) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"),
                columns: new[] { "AddressForename", "AddressSurname", "Created", "LastUpdated" },
                values: new object[] { "Lillie", "Harper", new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2690), new DateTime(2024, 5, 24, 16, 26, 48, 473, DateTimeKind.Local).AddTicks(2691) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressForename",
                table: "MSOS_Order");

            migrationBuilder.DropColumn(
                name: "AddressSurname",
                table: "MSOS_Order");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "MSOS_OrderItem",
                type: "decimal(19,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MSOS_OrderItem",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("07a2ab81-7651-484f-945e-08074e7662bc"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(871), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(873) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("30cfccfe-038c-4f20-a306-f0a2a9df0829"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(853), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(854) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("34335ca4-ebcd-4a2e-b54a-af9feb09535d"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(860), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(862) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("54d54c4e-d774-4b01-af87-0d0b94510767"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(866), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(867) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("69e3878e-8293-4e81-8791-31328e2a3907"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(883), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(884) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("6f3e950c-5502-491f-911d-02e112318705"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(838), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(839) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("7b5673fa-166e-417e-a0bf-d2c7d72b6ab3"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(781), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(783) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("9e93a116-a143-4865-8a84-b000d0df09c7"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(877), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(878) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("d3ca8ea8-97d6-41ce-937f-e2d9a905d61e"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(726), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(770) });

            migrationBuilder.UpdateData(
                table: "MSOS_Order",
                keyColumn: "Id",
                keyValue: new Guid("e7cc2320-6443-46bd-93f4-a2ae7b437287"),
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(843), new DateTime(2024, 5, 3, 16, 50, 24, 518, DateTimeKind.Local).AddTicks(845) });
        }
    }
}
