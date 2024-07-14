using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NullableField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("20f28140-61d3-4885-b07a-8d4107832be2"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("3a597164-7cf3-4fcf-a111-8e9d35c8cc00"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("ee41c86d-87ff-49cf-83d9-baae7db13bbb"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("023e549e-2cf7-4984-8cdc-4989ff9c5c2a"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("29d54d97-39f8-4301-b172-5d8454043d9e"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("34c7d5ff-10fc-430a-a0fc-cb112da46b62"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AcceptedBy",
                table: "ReturnRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // migrationBuilder.InsertData(
            //     table: "Categories",
            //     columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
            //     values: new object[,]
            //     {
            //         { new Guid("8c01888f-9cae-42c1-b23a-2512f487ee33"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" },
            //         { new Guid("c179579f-9d0f-4631-8f9f-53deb114ec40"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
            //         { new Guid("eac81f0a-d8a4-49cd-89d4-c08933602cae"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Users",
            //     columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
            //     values: new object[,]
            //     {
            //         { new Guid("1d100e61-7027-42d6-9aa8-28e2a3417351"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 13, 52, 46, 699, DateTimeKind.Unspecified).AddTicks(2765), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEJhBOUMHgC3Opp//VHpGkP4+ttcmaah51HbHHFfLoGDZirfNCcUgd5i7XbjKiODTdw==", 1, "adminHCM" },
            //         { new Guid("aae0a9ea-c2a3-4d5d-8905-761ade5572ff"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 13, 52, 46, 500, DateTimeKind.Unspecified).AddTicks(6543), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAENH7CmWKigTidC43UH5hLKqHsvcdFnxnRsdMAcvy2/KMaM7DZ2ymF6U/yKEDgWOV8Q==", 1, "adminHN" },
            //         { new Guid("be3173cf-91d7-4991-9f24-d5caedfb550b"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 13, 52, 46, 926, DateTimeKind.Unspecified).AddTicks(1762), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAEBXZpNmF1AJ71qjaYcVzxvFk6ZOMlmo54kRQMdBrackjgbwNpt3wO2aQqSdjoz2sfA==", 1, "adminDN" }
            //     });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8c01888f-9cae-42c1-b23a-2512f487ee33"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c179579f-9d0f-4631-8f9f-53deb114ec40"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("eac81f0a-d8a4-49cd-89d4-c08933602cae"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1d100e61-7027-42d6-9aa8-28e2a3417351"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aae0a9ea-c2a3-4d5d-8905-761ade5572ff"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("be3173cf-91d7-4991-9f24-d5caedfb550b"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AcceptedBy",
                table: "ReturnRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
                values: new object[,]
                {
                    { new Guid("20f28140-61d3-4885-b07a-8d4107832be2"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
                    { new Guid("3a597164-7cf3-4fcf-a111-8e9d35c8cc00"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" },
                    { new Guid("ee41c86d-87ff-49cf-83d9-baae7db13bbb"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("023e549e-2cf7-4984-8cdc-4989ff9c5c2a"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 9, 6, 33, 206, DateTimeKind.Unspecified).AddTicks(9731), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAEA2vI5KZK39+Yq61npnL3VZjdJf3pAgb9BNydZSrVse4l9xRoqWyLC4ITV7rYFci1A==", 1, "adminHN" },
                    { new Guid("29d54d97-39f8-4301-b172-5d8454043d9e"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 9, 6, 33, 678, DateTimeKind.Unspecified).AddTicks(5219), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAEJvydqlHc9cR7Gfj2v++h7J4sbDf1zoXGGN2MXE8BqG5OYba+BgO29qflUpueaJA4Q==", 1, "adminDN" },
                    { new Guid("34c7d5ff-10fc-430a-a0fc-cb112da46b62"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 9, 6, 33, 440, DateTimeKind.Unspecified).AddTicks(2168), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEHrhOMB8WQrWLl2mB2BnZ7vmTsIwQJJiWTLU7m/rw5PHMytLQQj/Q/lC0rxl8x5scg==", 1, "adminHCM" }
                });
        }
    }
}
