using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi_pred.Migrations
{
    /// <inheritdoc />
    public partial class addLogoPathToTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Teams");
        }
    }
}
