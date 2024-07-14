using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeUnnecessaryField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("26955460-0b2b-42c1-ba4c-288a2cab953e"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("3af5ac03-0c44-40a5-9594-1a65c9b1e4ce"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("54c478e1-0dcd-4ab6-acfc-ebdaec98bd95"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("98a9523a-c138-4d19-8498-ad8f7c4443b9"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("b2478120-f3e9-41f6-81b0-a952cdd3e865"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("cdfab219-2273-4a3a-81a8-f5da7c80750d"));

            migrationBuilder.DropColumn(
                name: "IsRequested",
                table: "Assignments");

            // migrationBuilder.InsertData(
            //     table: "Categories",
            //     columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
            //     values: new object[,]
            //     {
            //         { new Guid("6a70be78-84f7-4a6f-ab9b-320be20abe90"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" },
            //         { new Guid("78a990fa-3e7f-4a3e-836f-6b6b79694955"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
            //         { new Guid("fc9f0850-242a-4043-acd0-6eade6a067cb"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Users",
            //     columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
            //     values: new object[,]
            //     {
            //         { new Guid("87996093-7586-45b5-bd44-67ed9dde208e"), "System", new DateTimeOffset(new DateTime(2024, 7, 2, 13, 4, 51, 647, DateTimeKind.Unspecified).AddTicks(3252), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAEOagerVzBj2tWN1TmR7iFktbRSVj1A2NvOnMX+9axCWyfHdWr4uAGcBsCYpx5/W0Ug==", 1, "adminDN" },
            //         { new Guid("a4065ce5-58da-44b5-b3d7-47a89aa02474"), "System", new DateTimeOffset(new DateTime(2024, 7, 2, 13, 4, 51, 137, DateTimeKind.Unspecified).AddTicks(1250), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAEG3+fwgMYO7TCEIEB5PPfyCDCkSHmrQRP/D4SELA8WH60u38NIAyISL4Sp30sARq7A==", 1, "adminHN" },
            //         { new Guid("cbc111f4-fccb-497b-a072-bf2bee21121f"), "System", new DateTimeOffset(new DateTime(2024, 7, 2, 13, 4, 51, 390, DateTimeKind.Unspecified).AddTicks(3665), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEOLY5SyFu3CK+2o0M1byQgUbPS1r8GQG7YzsGoBm5wxvuHDPh+OIBosYphJNUgaQGw==", 1, "adminHCM" }
            //     });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6a70be78-84f7-4a6f-ab9b-320be20abe90"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("78a990fa-3e7f-4a3e-836f-6b6b79694955"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fc9f0850-242a-4043-acd0-6eade6a067cb"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("87996093-7586-45b5-bd44-67ed9dde208e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a4065ce5-58da-44b5-b3d7-47a89aa02474"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cbc111f4-fccb-497b-a072-bf2bee21121f"));

            migrationBuilder.AddColumn<bool>(
                name: "IsRequested",
                table: "Assignments",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
                values: new object[,]
                {
                    { new Guid("26955460-0b2b-42c1-ba4c-288a2cab953e"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" },
                    { new Guid("3af5ac03-0c44-40a5-9594-1a65c9b1e4ce"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
                    { new Guid("54c478e1-0dcd-4ab6-acfc-ebdaec98bd95"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("98a9523a-c138-4d19-8498-ad8f7c4443b9"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 16, 43, 26, 751, DateTimeKind.Unspecified).AddTicks(5826), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAEFSnreB93FNQEzGhf5fXNsUwzE7+W5mXb3O9T/3z4kbAswXxAMLJWZYgwTNqxPrSEw==", 1, "adminHN" },
                    { new Guid("b2478120-f3e9-41f6-81b0-a952cdd3e865"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 16, 43, 26, 988, DateTimeKind.Unspecified).AddTicks(407), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEKatSV1ZZgG5OsZCRgTyD5uDraERI+A1j8jLRSvXDLtRyKAat0vu3fVDTtNFMdetPw==", 1, "adminHCM" },
                    { new Guid("cdfab219-2273-4a3a-81a8-f5da7c80750d"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 16, 43, 27, 240, DateTimeKind.Unspecified).AddTicks(4965), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAEDBpMohQ5wnUROg0Ny693Fv+gnVqkp147zp5gMpvkHHtsEnovai/7/o9SigI8THshw==", 1, "adminDN" }
                });
        }
    }
}
