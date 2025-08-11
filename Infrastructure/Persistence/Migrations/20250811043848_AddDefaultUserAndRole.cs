using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultUserAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDisabled", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019893ad-3b10-7e54-9964-8b6257795613", "019893ad-3b10-7e54-9964-8b616f692462", false, false, "Admin", "ADMIN" },
                    { "019893b8-e9fe-7104-aa28-a332881d483c", "019893b8-e9fe-7104-aa28-a333eb5b527b", true, false, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmationCode", "EmailConfirmationCodeExpiration", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "019893ad-3b0f-74e1-ac54-533919ccb36d", 0, "019893ad-3b0f-74e1-ac54-533a1860b770", "Zyara@admin.com", null, null, true, "Zyara", false, "Admin", false, null, "ZYARA@ADMIN.COM", "ZYARA", "AQAAAAIAAYagAAAAEIdrL7oVMEOdyzkAbmEyq2wa9j+QIoEetOOiH+N1DWS8g7AX/BLiwAi/z4jmt7xEzA==", null, false, "38C841ED0FA840F1BED8AFDA745C6401", false, "Zyara" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019893ad-3b10-7e54-9964-8b6257795613", "019893ad-3b0f-74e1-ac54-533919ccb36d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019893b8-e9fe-7104-aa28-a332881d483c");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019893ad-3b10-7e54-9964-8b6257795613", "019893ad-3b0f-74e1-ac54-533919ccb36d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019893ad-3b10-7e54-9964-8b6257795613");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019893ad-3b0f-74e1-ac54-533919ccb36d");
        }
    }
}
