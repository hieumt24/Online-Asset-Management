using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NullableReturnedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("6a70be78-84f7-4a6f-ab9b-320be20abe90"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("78a990fa-3e7f-4a3e-836f-6b6b79694955"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("fc9f0850-242a-4043-acd0-6eade6a067cb"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("87996093-7586-45b5-bd44-67ed9dde208e"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("a4065ce5-58da-44b5-b3d7-47a89aa02474"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("cbc111f4-fccb-497b-a072-bf2bee21121f"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnedDate",
                table: "ReturnRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            // migrationBuilder.InsertData(
            //     table: "Categories",
            //     columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
            //     values: new object[,]
            //     {
            //         { new Guid("7b5a0f05-5ebb-4439-b7d1-64084851d550"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" },
            //         { new Guid("7c4b15ec-7c1a-400d-b11a-b0e735f4f270"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
            //         { new Guid("8e41ab87-baf7-4df9-ba72-e405c362bdf1"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Users",
            //     columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
            //     values: new object[,]
            //     {
            //         { new Guid("3c90ef18-ad48-44a8-9e07-1b354fbee637"), "System", new DateTimeOffset(new DateTime(2024, 7, 2, 16, 3, 59, 304, DateTimeKind.Unspecified).AddTicks(6072), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAEHDBnUDxf4ZnqbDBO2x34bz3yXFEE23AzJ545Ij7xBZO83IpQCHukjce12Cz0AKdcA==", 1, "adminHN" },
            //         { new Guid("882831cf-243b-4fcd-842f-21de5d013af0"), "System", new DateTimeOffset(new DateTime(2024, 7, 2, 16, 3, 59, 488, DateTimeKind.Unspecified).AddTicks(1227), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEMxExIrWCqi27CYArqRYQFN23NZhmVfHxMrGmK2ig5qaWfqX6KxPYVM2YQiOxD+zIg==", 1, "adminHCM" },
            //         { new Guid("c2939d12-3e22-4003-a1af-eadda06bd90e"), "System", new DateTimeOffset(new DateTime(2024, 7, 2, 16, 3, 59, 716, DateTimeKind.Unspecified).AddTicks(2612), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAEIxl4wGZSOfeoOz7nfUE/6qOSi9WqLMn9MdT7g7i9XxsuMxDJmeHvLGcm2SLYRwUjA==", 1, "adminDN" }
            //     });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7b5a0f05-5ebb-4439-b7d1-64084851d550"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7c4b15ec-7c1a-400d-b11a-b0e735f4f270"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8e41ab87-baf7-4df9-ba72-e405c362bdf1"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3c90ef18-ad48-44a8-9e07-1b354fbee637"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("882831cf-243b-4fcd-842f-21de5d013af0"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c2939d12-3e22-4003-a1af-eadda06bd90e"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnedDate",
                table: "ReturnRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
                values: new object[,]
                {
                    { new Guid("6a70be78-84f7-4a6f-ab9b-320be20abe90"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" },
                    { new Guid("78a990fa-3e7f-4a3e-836f-6b6b79694955"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
                    { new Guid("fc9f0850-242a-4043-acd0-6eade6a067cb"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("87996093-7586-45b5-bd44-67ed9dde208e"), "System", new DateTimeOffset(new DateTime(2024, 7, 2, 13, 4, 51, 647, DateTimeKind.Unspecified).AddTicks(3252), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAEOagerVzBj2tWN1TmR7iFktbRSVj1A2NvOnMX+9axCWyfHdWr4uAGcBsCYpx5/W0Ug==", 1, "adminDN" },
                    { new Guid("a4065ce5-58da-44b5-b3d7-47a89aa02474"), "System", new DateTimeOffset(new DateTime(2024, 7, 2, 13, 4, 51, 137, DateTimeKind.Unspecified).AddTicks(1250), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAEG3+fwgMYO7TCEIEB5PPfyCDCkSHmrQRP/D4SELA8WH60u38NIAyISL4Sp30sARq7A==", 1, "adminHN" },
                    { new Guid("cbc111f4-fccb-497b-a072-bf2bee21121f"), "System", new DateTimeOffset(new DateTime(2024, 7, 2, 13, 4, 51, 390, DateTimeKind.Unspecified).AddTicks(3665), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEOLY5SyFu3CK+2o0M1byQgUbPS1r8GQG7YzsGoBm5wxvuHDPh+OIBosYphJNUgaQGw==", 1, "adminHCM" }
                });
        }
    }
}
