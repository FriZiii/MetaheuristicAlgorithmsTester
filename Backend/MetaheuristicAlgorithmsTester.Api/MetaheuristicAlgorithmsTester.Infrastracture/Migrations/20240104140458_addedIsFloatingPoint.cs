using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaheuristicAlgorithmsTester.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addedIsFloatingPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFloatingPoint",
                table: "Parameters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFloatingPoint",
                table: "Parameters");
        }
    }
}
