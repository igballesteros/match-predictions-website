using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi_pred.Migrations
{
    /// <inheritdoc />
    public partial class FixPointsNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("UPDATE Users SET Points = 0 WHERE Points IS NULL");

            migrationBuilder.AlterColumn<int>(
                name: "Points",
                table: "Users",
                type: "int",
                nullable: false,        // Changing to NOT NULL
                defaultValue: 0,        // Default value when not specified
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,      // Old: nullable
                oldDefaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Points",
                table: "Users",
                type: "int",
                nullable: true,         // Reverting back to nullable
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}
