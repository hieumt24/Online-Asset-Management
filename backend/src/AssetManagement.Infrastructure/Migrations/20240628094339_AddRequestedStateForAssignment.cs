using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestedStateForAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("2a03cdb3-72be-48f4-891b-4b5462e82652"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("c17a1060-a337-4e29-ab02-c57f232dfeab"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("df0cc854-f846-4bda-920c-bf44ab8feab4"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("1ca3c4a0-9fbb-4400-8a5a-92935642d683"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("5982babe-ad47-40d5-acaa-99b6ee5329fc"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("dae1f2cf-7b3c-446f-9584-e2ed37ac055c"));

            migrationBuilder.AddColumn<bool>(
                name: "IsRequested",
                table: "Assignments",
                type: "bit",
                nullable: true);

            // migrationBuilder.InsertData(
            //     table: "Categories",
            //     columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
            //     values: new object[,]
            //     {
            //         { new Guid("26955460-0b2b-42c1-ba4c-288a2cab953e"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" },
            //         { new Guid("3af5ac03-0c44-40a5-9594-1a65c9b1e4ce"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
            //         { new Guid("54c478e1-0dcd-4ab6-acfc-ebdaec98bd95"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Users",
            //     columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
            //     values: new object[,]
            //     {
            //         { new Guid("98a9523a-c138-4d19-8498-ad8f7c4443b9"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 16, 43, 26, 751, DateTimeKind.Unspecified).AddTicks(5826), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAEFSnreB93FNQEzGhf5fXNsUwzE7+W5mXb3O9T/3z4kbAswXxAMLJWZYgwTNqxPrSEw==", 1, "adminHN" },
            //         { new Guid("b2478120-f3e9-41f6-81b0-a952cdd3e865"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 16, 43, 26, 988, DateTimeKind.Unspecified).AddTicks(407), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEKatSV1ZZgG5OsZCRgTyD5uDraERI+A1j8jLRSvXDLtRyKAat0vu3fVDTtNFMdetPw==", 1, "adminHCM" },
            //         { new Guid("cdfab219-2273-4a3a-81a8-f5da7c80750d"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 16, 43, 27, 240, DateTimeKind.Unspecified).AddTicks(4965), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAEDBpMohQ5wnUROg0Ny693Fv+gnVqkp147zp5gMpvkHHtsEnovai/7/o9SigI8THshw==", 1, "adminDN" }
            //     });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("26955460-0b2b-42c1-ba4c-288a2cab953e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3af5ac03-0c44-40a5-9594-1a65c9b1e4ce"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("54c478e1-0dcd-4ab6-acfc-ebdaec98bd95"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98a9523a-c138-4d19-8498-ad8f7c4443b9"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b2478120-f3e9-41f6-81b0-a952cdd3e865"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cdfab219-2273-4a3a-81a8-f5da7c80750d"));

            migrationBuilder.DropColumn(
                name: "IsRequested",
                table: "Assignments");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
                values: new object[,]
                {
                    { new Guid("2a03cdb3-72be-48f4-891b-4b5462e82652"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" },
                    { new Guid("c17a1060-a337-4e29-ab02-c57f232dfeab"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
                    { new Guid("df0cc854-f846-4bda-920c-bf44ab8feab4"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("1ca3c4a0-9fbb-4400-8a5a-92935642d683"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 11, 43, 12, 496, DateTimeKind.Unspecified).AddTicks(8327), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAELVNRsU2rGSqaIXT1HaiJfFWhr8WavnP4rVd5B4kaFU01YzzxBpE36KizP8VbIN6lw==", 1, "adminDN" },
                    { new Guid("5982babe-ad47-40d5-acaa-99b6ee5329fc"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 11, 43, 12, 238, DateTimeKind.Unspecified).AddTicks(6825), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEEa8Z5MMu1L3brt/dAAVJOuIEoS/nWjR6MweILh0xAiAJ81di10+TsAgK0XkAfGBUw==", 1, "adminHCM" },
                    { new Guid("dae1f2cf-7b3c-446f-9584-e2ed37ac055c"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 11, 43, 11, 987, DateTimeKind.Unspecified).AddTicks(6166), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAECwQv1hUuMnmyWvOQaF29dPRr4YwlOgrSi+PTO/nNoCPsOxJGHeOsz8/+fvMXRAaug==", 1, "adminHN" }
                });
        }
    }
}
