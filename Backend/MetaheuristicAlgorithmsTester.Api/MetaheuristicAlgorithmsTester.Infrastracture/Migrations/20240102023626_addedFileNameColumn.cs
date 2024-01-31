using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaheuristicAlgorithmsTester.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addedFileNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Algorithms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Algorithms");
        }
    }
}
