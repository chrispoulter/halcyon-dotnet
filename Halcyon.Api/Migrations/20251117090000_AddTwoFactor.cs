using Microsoft.EntityFrameworkCore.Migrations;

namespace Halcyon.Api.Migrations;

public partial class AddTwoFactor : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsTwoFactorEnabled",
            table: "Users",
            type: "boolean",
            nullable: false,
            defaultValue: false
        );

        migrationBuilder.AddColumn<string>(
            name: "TwoFactorSecret",
            table: "Users",
            type: "text",
            nullable: true
        );

        migrationBuilder.AddColumn<string>(
            name: "TwoFactorTempSecret",
            table: "Users",
            type: "text",
            nullable: true
        );

        migrationBuilder.AddColumn<string[]>(
            name: "TwoFactorRecoveryCodes",
            table: "Users",
            type: "text[]",
            nullable: true
        );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "IsTwoFactorEnabled", table: "Users");
        migrationBuilder.DropColumn(name: "TwoFactorSecret", table: "Users");
        migrationBuilder.DropColumn(name: "TwoFactorTempSecret", table: "Users");
        migrationBuilder.DropColumn(name: "TwoFactorRecoveryCodes", table: "Users");
    }
}
