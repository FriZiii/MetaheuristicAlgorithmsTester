using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaheuristicAlgorithmsTester.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addedElaspedTimeForSingle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExecutionTime",
                table: "ExecutedSingleAlgorithms",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionTime",
                table: "ExecutedSingleAlgorithms");
        }
    }
}
