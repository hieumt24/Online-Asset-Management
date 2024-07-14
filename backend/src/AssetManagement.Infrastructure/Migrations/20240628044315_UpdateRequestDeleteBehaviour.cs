using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequestDeleteBehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRequests_Assignments_AssignmentId",
                table: "ReturnRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReturnRequests_AssignmentId",
                table: "ReturnRequests");

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("8c01888f-9cae-42c1-b23a-2512f487ee33"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("c179579f-9d0f-4631-8f9f-53deb114ec40"));

            // migrationBuilder.DeleteData(
            //     table: "Categories",
            //     keyColumn: "Id",
            //     keyValue: new Guid("eac81f0a-d8a4-49cd-89d4-c08933602cae"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("1d100e61-7027-42d6-9aa8-28e2a3417351"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("aae0a9ea-c2a3-4d5d-8905-761ade5572ff"));

            // migrationBuilder.DeleteData(
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: new Guid("be3173cf-91d7-4991-9f24-d5caedfb550b"));

            // migrationBuilder.InsertData(
            //     table: "Categories",
            //     columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
            //     values: new object[,]
            //     {
            //         { new Guid("2a03cdb3-72be-48f4-891b-4b5462e82652"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" },
            //         { new Guid("c17a1060-a337-4e29-ab02-c57f232dfeab"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
            //         { new Guid("df0cc854-f846-4bda-920c-bf44ab8feab4"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" }
            //     });

            // migrationBuilder.InsertData(
            //     table: "Users",
            //     columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
            //     values: new object[,]
            //     {
            //         { new Guid("1ca3c4a0-9fbb-4400-8a5a-92935642d683"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 11, 43, 12, 496, DateTimeKind.Unspecified).AddTicks(8327), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAELVNRsU2rGSqaIXT1HaiJfFWhr8WavnP4rVd5B4kaFU01YzzxBpE36KizP8VbIN6lw==", 1, "adminDN" },
            //         { new Guid("5982babe-ad47-40d5-acaa-99b6ee5329fc"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 11, 43, 12, 238, DateTimeKind.Unspecified).AddTicks(6825), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEEa8Z5MMu1L3brt/dAAVJOuIEoS/nWjR6MweILh0xAiAJ81di10+TsAgK0XkAfGBUw==", 1, "adminHCM" },
            //         { new Guid("dae1f2cf-7b3c-446f-9584-e2ed37ac055c"), "System", new DateTimeOffset(new DateTime(2024, 6, 28, 11, 43, 11, 987, DateTimeKind.Unspecified).AddTicks(6166), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAECwQv1hUuMnmyWvOQaF29dPRr4YwlOgrSi+PTO/nNoCPsOxJGHeOsz8/+fvMXRAaug==", 1, "adminHN" }
            //     });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequests_AssignmentId",
                table: "ReturnRequests",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRequests_Assignments_AssignmentId",
                table: "ReturnRequests",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRequests_Assignments_AssignmentId",
                table: "ReturnRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReturnRequests_AssignmentId",
                table: "ReturnRequests");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2a03cdb3-72be-48f4-891b-4b5462e82652"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c17a1060-a337-4e29-ab02-c57f232dfeab"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("df0cc854-f846-4bda-920c-bf44ab8feab4"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1ca3c4a0-9fbb-4400-8a5a-92935642d683"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5982babe-ad47-40d5-acaa-99b6ee5329fc"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dae1f2cf-7b3c-446f-9584-e2ed37ac055c"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
                values: new object[,]
                {
                    { new Guid("8c01888f-9cae-42c1-b23a-2512f487ee33"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" },
                    { new Guid("c179579f-9d0f-4631-8f9f-53deb114ec40"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
                    { new Guid("eac81f0a-d8a4-49cd-89d4-c08933602cae"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("1d100e61-7027-42d6-9aa8-28e2a3417351"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 13, 52, 46, 699, DateTimeKind.Unspecified).AddTicks(2765), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEJhBOUMHgC3Opp//VHpGkP4+ttcmaah51HbHHFfLoGDZirfNCcUgd5i7XbjKiODTdw==", 1, "adminHCM" },
                    { new Guid("aae0a9ea-c2a3-4d5d-8905-761ade5572ff"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 13, 52, 46, 500, DateTimeKind.Unspecified).AddTicks(6543), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAENH7CmWKigTidC43UH5hLKqHsvcdFnxnRsdMAcvy2/KMaM7DZ2ymF6U/yKEDgWOV8Q==", 1, "adminHN" },
                    { new Guid("be3173cf-91d7-4991-9f24-d5caedfb550b"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 13, 52, 46, 926, DateTimeKind.Unspecified).AddTicks(1762), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAEBXZpNmF1AJ71qjaYcVzxvFk6ZOMlmo54kRQMdBrackjgbwNpt3wO2aQqSdjoz2sfA==", 1, "adminDN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequests_AssignmentId",
                table: "ReturnRequests",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRequests_Assignments_AssignmentId",
                table: "ReturnRequests",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
