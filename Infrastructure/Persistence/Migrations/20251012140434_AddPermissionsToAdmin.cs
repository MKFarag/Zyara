using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionsToAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "delivery-man:read", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 2, "permissions", "delivery-man:read-orders", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 3, "permissions", "delivery-man:add", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 4, "permissions", "delivery-man:modify", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 5, "permissions", "delivery-man:set-orders", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 6, "permissions", "delivery-man:update-status", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 7, "permissions", "order:read-all", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 8, "permissions", "order:read-earning", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 9, "permissions", "order:update-status", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 10, "permissions", "product:add", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 11, "permissions", "product:modify", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 12, "permissions", "product:modify-price", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 13, "permissions", "product:modify-discount", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 14, "permissions", "product:modify-quantity", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 15, "permissions", "role:read", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 16, "permissions", "role:add", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 17, "permissions", "role:modify", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 18, "permissions", "role:update-status", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 19, "permissions", "user:read", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 20, "permissions", "user:add", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 21, "permissions", "user:modify", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 22, "permissions", "user:update-status", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" },
                    { 23, "permissions", "user:unlock", "0198fe4f-b61c-78ab-9ceb-33fd75389eb8" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);
        }
    }
}
