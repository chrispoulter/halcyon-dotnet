using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Halcyon.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoFactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_two_factor_enabled",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<string[]>(
                name: "two_factor_recovery_codes",
                table: "users",
                type: "text[]",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "two_factor_secret",
                table: "users",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "two_factor_temp_secret",
                table: "users",
                type: "text",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "is_two_factor_enabled", table: "users");

            migrationBuilder.DropColumn(name: "two_factor_recovery_codes", table: "users");

            migrationBuilder.DropColumn(name: "two_factor_secret", table: "users");

            migrationBuilder.DropColumn(name: "two_factor_temp_secret", table: "users");
        }
    }
}
