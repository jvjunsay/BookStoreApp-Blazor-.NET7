using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUserandRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32d8ef80-f712-4adf-a7a3-912867d25140", null, "Administrator", "ADMINISTRATOR" },
                    { "35699a1f-cf8b-461f-8b3b-539f8f43ba21", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "2b7e994f-4eb2-4a6c-9ca9-ffe815b1a322", 0, "e80210e7-3aed-43fb-86f1-e74e03af080f", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEIa5Oucs2i09/plJfG1WHnsVJBmegBIovjdEfSUJ5rpys+Dk64MrLkk9uEbvgO/mDw==", null, false, "45f367ba-b340-4839-9942-2ad5a98d83b2", false, "admin@bookstore.com" },
                    { "d4baa2b0-6092-44f1-a2e4-6b1ebabc687e", 0, "da713ac9-4fe4-4d69-8625-6f4ee86d669c", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "User@BOOKSTORE.COM", "AQAAAAIAAYagAAAAECUrcEWMjgS3yT3jvHRDPhUK54/YJRoq03/Ti0TdR0Sh4sArhRIiWhyG9ZUzNLFz/w==", null, false, "730e6578-bd02-4c6f-8c45-58c772e1700e", false, "user@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "32d8ef80-f712-4adf-a7a3-912867d25140", "2b7e994f-4eb2-4a6c-9ca9-ffe815b1a322" },
                    { "35699a1f-cf8b-461f-8b3b-539f8f43ba21", "d4baa2b0-6092-44f1-a2e4-6b1ebabc687e" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "32d8ef80-f712-4adf-a7a3-912867d25140", "2b7e994f-4eb2-4a6c-9ca9-ffe815b1a322" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "35699a1f-cf8b-461f-8b3b-539f8f43ba21", "d4baa2b0-6092-44f1-a2e4-6b1ebabc687e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32d8ef80-f712-4adf-a7a3-912867d25140");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35699a1f-cf8b-461f-8b3b-539f8f43ba21");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2b7e994f-4eb2-4a6c-9ca9-ffe815b1a322");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4baa2b0-6092-44f1-a2e4-6b1ebabc687e");
        }
    }
}
