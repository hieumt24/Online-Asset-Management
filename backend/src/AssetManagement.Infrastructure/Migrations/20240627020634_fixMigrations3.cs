using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixMigrations3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "Categories",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         CategoryName = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         Prefix = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //         LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //         IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Categories", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Users",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //         LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //         DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         Gender = table.Column<int>(type: "int", nullable: false),
            //         Role = table.Column<int>(type: "int", nullable: false),
            //         StaffCode = table.Column<string>(type: "nvarchar(max)", nullable: false, computedColumnSql: "CONCAT('SD', RIGHT('0000' + CAST(StaffCodeId AS VARCHAR(4)), 4))"),
            //         StaffCodeId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Location = table.Column<int>(type: "int", nullable: false),
            //         IsFirstTimeLogin = table.Column<bool>(type: "bit", nullable: false),
            //         CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //         LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //         IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Users", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Assets",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         AssetCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         AssetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Specification = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         InstalledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         State = table.Column<int>(type: "int", nullable: false),
            //         AssetLocation = table.Column<int>(type: "int", nullable: false),
            //         CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //         LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //         IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Assets", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Assets_Categories_CategoryId",
            //             column: x => x.CategoryId,
            //             principalTable: "Categories",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Assignments",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         AssignedIdTo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         AssignedIdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         ReturnRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //         Location = table.Column<int>(type: "int", nullable: true),
            //         State = table.Column<int>(type: "int", nullable: false),
            //         Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //         LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //         IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Assignments", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Assignments_Assets_AssetId",
            //             column: x => x.AssetId,
            //             principalTable: "Assets",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Assignments_Users_AssignedIdBy",
            //             column: x => x.AssignedIdBy,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Assignments_Users_AssignedIdTo",
            //             column: x => x.AssignedIdTo,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ReturnRequests",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         AssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         AcceptedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //         ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         Location = table.Column<int>(type: "int", nullable: false),
            //         ReturnState = table.Column<int>(type: "int", nullable: false),
            //         CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //         LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //         IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ReturnRequests", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_ReturnRequests_Assignments_AssignmentId",
            //             column: x => x.AssignmentId,
            //             principalTable: "Assignments",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_ReturnRequests_Users_AcceptedBy",
            //             column: x => x.AcceptedBy,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_ReturnRequests_Users_RequestedBy",
            //             column: x => x.RequestedBy,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            //migrationBuilder.InsertData(
            //    table: "Categories",
            //    columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedOn", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "Prefix" },
            //    values: new object[,]
            //    {
            //        { new Guid("20f28140-61d3-4885-b07a-8d4107832be2"), "Monitor", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "MO" },
            //        { new Guid("3a597164-7cf3-4fcf-a111-8e9d35c8cc00"), "Laptop", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "LA" },
            //        { new Guid("ee41c86d-87ff-49cf-83d9-baae7db13bbb"), "Desk", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "DE" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "Users",
            //    columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "IsFirstTimeLogin", "JoinedDate", "LastModifiedBy", "LastModifiedOn", "LastName", "Location", "PasswordHash", "Role", "Username" },
            //    values: new object[,]
            //    {
            //        { new Guid("023e549e-2cf7-4984-8cdc-4989ff9c5c2a"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 9, 6, 33, 206, DateTimeKind.Unspecified).AddTicks(9731), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ha Noi", 1, "AQAAAAIAAYagAAAAEA2vI5KZK39+Yq61npnL3VZjdJf3pAgb9BNydZSrVse4l9xRoqWyLC4ITV7rYFci1A==", 1, "adminHN" },
            //        { new Guid("29d54d97-39f8-4301-b172-5d8454043d9e"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 9, 6, 33, 678, DateTimeKind.Unspecified).AddTicks(5219), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Da Nang", 2, "AQAAAAIAAYagAAAAEJvydqlHc9cR7Gfj2v++h7J4sbDf1zoXGGN2MXE8BqG5OYba+BgO29qflUpueaJA4Q==", 1, "adminDN" },
            //        { new Guid("34c7d5ff-10fc-430a-a0fc-cb112da46b62"), "System", new DateTimeOffset(new DateTime(2024, 6, 27, 9, 6, 33, 440, DateTimeKind.Unspecified).AddTicks(2168), new TimeSpan(0, 7, 0, 0, 0)), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 0, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ho Chi Minh", 3, "AQAAAAIAAYagAAAAEHrhOMB8WQrWLl2mB2BnZ7vmTsIwQJJiWTLU7m/rw5PHMytLQQj/Q/lC0rxl8x5scg==", 1, "adminHCM" }
            //    });

        //     migrationBuilder.CreateIndex(
        //         name: "IX_Assets_CategoryId",
        //         table: "Assets",
        //         column: "CategoryId");

        //     migrationBuilder.CreateIndex(
        //         name: "IX_Assignments_AssetId",
        //         table: "Assignments",
        //         column: "AssetId");

        //     migrationBuilder.CreateIndex(
        //         name: "IX_Assignments_AssignedIdBy",
        //         table: "Assignments",
        //         column: "AssignedIdBy");

        //     migrationBuilder.CreateIndex(
        //         name: "IX_Assignments_AssignedIdTo",
        //         table: "Assignments",
        //         column: "AssignedIdTo");

        //     migrationBuilder.CreateIndex(
        //         name: "IX_Categories_CategoryName",
        //         table: "Categories",
        //         column: "CategoryName",
        //         unique: true);

        //     migrationBuilder.CreateIndex(
        //         name: "IX_Categories_Prefix",
        //         table: "Categories",
        //         column: "Prefix",
        //         unique: true);

        //     migrationBuilder.CreateIndex(
        //         name: "IX_ReturnRequests_AcceptedBy",
        //         table: "ReturnRequests",
        //         column: "AcceptedBy");

        //     migrationBuilder.CreateIndex(
        //         name: "IX_ReturnRequests_AssignmentId",
        //         table: "ReturnRequests",
        //         column: "AssignmentId",
        //         unique: true);

        //     migrationBuilder.CreateIndex(
        //         name: "IX_ReturnRequests_RequestedBy",
        //         table: "ReturnRequests",
        //         column: "RequestedBy");

        //     migrationBuilder.CreateIndex(
        //         name: "IX_Users_Username",
        //         table: "Users",
        //         column: "Username",
        //         unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnRequests");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
