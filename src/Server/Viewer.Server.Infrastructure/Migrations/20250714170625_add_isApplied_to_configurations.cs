using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Viewer.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_isApplied_to_configurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApplied",
                table: "Configurations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApplied",
                table: "Configurations");
        }
    }
}
