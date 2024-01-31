using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaheuristicAlgorithmsTester.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addedSatisfiedResultAndDimesion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Dimension",
                table: "ExecutedMultipleAlgorithms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SatisfiedResult",
                table: "ExecutedMultipleAlgorithms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimension",
                table: "ExecutedMultipleAlgorithms");

            migrationBuilder.DropColumn(
                name: "SatisfiedResult",
                table: "ExecutedMultipleAlgorithms");
        }
    }
}
