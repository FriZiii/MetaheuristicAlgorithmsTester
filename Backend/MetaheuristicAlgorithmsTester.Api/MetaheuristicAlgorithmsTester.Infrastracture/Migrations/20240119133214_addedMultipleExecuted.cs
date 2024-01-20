using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaheuristicAlgorithmsTester.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addedMultipleExecuted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutedAlgorithms");

            migrationBuilder.CreateTable(
                name: "ExecutedMultipleAlgorithms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MultipleTestId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TestedAlgorithmId = table.Column<int>(type: "int", nullable: false),
                    TestedAlgorithmName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestedFitnessFunctionId = table.Column<int>(type: "int", nullable: false),
                    TestedFitnessFunctionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XBest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FBest = table.Column<double>(type: "float", nullable: true),
                    NumberOfEvaluationFitnessFunction = table.Column<int>(type: "int", nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFailed = table.Column<bool>(type: "bit", nullable: false),
                    AlgorithmStateFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimerFrequency = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutedMultipleAlgorithms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExecutedSingleAlgorithms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TestedAlgorithmId = table.Column<int>(type: "int", nullable: false),
                    TestedAlgorithmName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestedFitnessFunctionId = table.Column<int>(type: "int", nullable: false),
                    TestedFitnessFunctionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XBest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FBest = table.Column<double>(type: "float", nullable: true),
                    NumberOfEvaluationFitnessFunction = table.Column<int>(type: "int", nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFailed = table.Column<bool>(type: "bit", nullable: false),
                    AlgorithmStateFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimerFrequency = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutedSingleAlgorithms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutedMultipleAlgorithms");

            migrationBuilder.DropTable(
                name: "ExecutedSingleAlgorithms");

            migrationBuilder.CreateTable(
                name: "ExecutedAlgorithms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlgorithmStateFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    FBest = table.Column<double>(type: "float", nullable: true),
                    IsFailed = table.Column<bool>(type: "bit", nullable: true),
                    NumberOfEvaluationFitnessFunction = table.Column<int>(type: "int", nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestedAlgorithmId = table.Column<int>(type: "int", nullable: false),
                    TestedAlgorithmName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestedFitnessFunctionId = table.Column<int>(type: "int", nullable: false),
                    TestedFitnessFunctionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimerFrequency = table.Column<int>(type: "int", nullable: true),
                    XBest = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutedAlgorithms", x => x.Id);
                });
        }
    }
}
