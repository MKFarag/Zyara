using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDisabled", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0198fe4f-b61c-78ab-9ceb-33fd75389eb8", "0198fe4f-b61c-78ab-9ceb-33fe69cd9b53", false, false, "Admin", "ADMIN" },
                    { "0198fe4f-b61c-78ab-9ceb-33ff9982ca29", "0198fe4f-b61c-78ab-9ceb-3400398ffaa8", true, false, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0198fe4f-b61c-78ab-9ceb-33fbfc4822b1", 0, "0198fe4f-b61c-78ab-9ceb-33fc74d9a1b7", "Zyara@admin.com", true, "Zyara", false, "Admin", false, null, "ZYARA@ADMIN.COM", "ZYARA", "AQAAAAIAAYagAAAAEMfH4k2juBLnlEzDH7A+c+W1J/CdHuy/Hb0F/apEtpaE+vHiPTwOZeUG+2MpHjgdOw==", null, false, "294957E7C1924F96BF38B7FBEAC53B83", false, "Zyara" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0198fe4f-b61c-78ab-9ceb-33fd75389eb8", "0198fe4f-b61c-78ab-9ceb-33fbfc4822b1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0198fe4f-b61c-78ab-9ceb-33ff9982ca29");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0198fe4f-b61c-78ab-9ceb-33fd75389eb8", "0198fe4f-b61c-78ab-9ceb-33fbfc4822b1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0198fe4f-b61c-78ab-9ceb-33fd75389eb8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0198fe4f-b61c-78ab-9ceb-33fbfc4822b1");
        }
    }
}
