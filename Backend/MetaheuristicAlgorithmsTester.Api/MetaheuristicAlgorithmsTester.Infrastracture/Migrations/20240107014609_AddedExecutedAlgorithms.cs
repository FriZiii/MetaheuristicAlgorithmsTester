using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaheuristicAlgorithmsTester.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddedExecutedAlgorithms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutedAlgorithms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestedAlgorithmId = table.Column<int>(type: "int", nullable: false),
                    TestedAlgorithmName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestedFitnessFunctionId = table.Column<int>(type: "int", nullable: false),
                    TestedFitnessFunctionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XBest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FBest = table.Column<double>(type: "float", nullable: false),
                    NumberOfEvaluationFitnessFunction = table.Column<int>(type: "int", nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutedAlgorithms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutedAlgorithms");
        }
    }
}
