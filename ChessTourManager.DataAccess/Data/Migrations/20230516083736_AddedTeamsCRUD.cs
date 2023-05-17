using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessTourManager.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTeamsCRUD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "team_attribute",
                table: "teams",
                type: "character(3)",
                fixedLength: true,
                maxLength: 3,
                nullable: true,
                defaultValueSql: "'   '::bpchar",
                oldClrType: typeof(string),
                oldType: "character(3)",
                oldFixedLength: true,
                oldMaxLength: 3,
                oldDefaultValueSql: "'   '::bpchar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "team_attribute",
                table: "teams",
                type: "character(3)",
                fixedLength: true,
                maxLength: 3,
                nullable: false,
                defaultValueSql: "'   '::bpchar",
                oldClrType: typeof(string),
                oldType: "character(3)",
                oldFixedLength: true,
                oldMaxLength: 3,
                oldNullable: true,
                oldDefaultValueSql: "'   '::bpchar");
        }
    }
}
