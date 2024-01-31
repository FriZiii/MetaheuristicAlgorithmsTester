﻿// <auto-generated />
using System;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MetaheuristicAlgorithmsTester.Infrastracture.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240121180119_addedExecutionTimeForMultiple")]
    partial class addedExecutionTimeForMultiple
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MetaheuristicAlgorithmsTester.Domain.Entities.Algorithm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Algorithms");
                });

            modelBuilder.Entity("MetaheuristicAlgorithmsTester.Domain.Entities.ExecutedMultipleAlgorithms", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AlgorithmStateFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("Depth")
                        .HasColumnType("int");

                    b.Property<int>("Dimension")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("ExecutionTime")
                        .HasColumnType("time");

                    b.Property<double?>("FBest")
                        .HasColumnType("float");

                    b.Property<bool>("IsFailed")
                        .HasColumnType("bit");

                    b.Property<string>("MultipleTestId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumberOfEvaluationFitnessFunction")
                        .HasColumnType("int");

                    b.Property<string>("Parameters")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SatisfiedResult")
                        .HasColumnType("float");

                    b.Property<int>("TestedAlgorithmId")
                        .HasColumnType("int");

                    b.Property<string>("TestedAlgorithmName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestedFitnessFunctionId")
                        .HasColumnType("int");

                    b.Property<string>("TestedFitnessFunctionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TimerFrequency")
                        .HasColumnType("int");

                    b.Property<string>("XBest")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ExecutedMultipleAlgorithms");
                });

            modelBuilder.Entity("MetaheuristicAlgorithmsTester.Domain.Entities.ExecutedSingleAlgorithm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AlgorithmStateFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<TimeSpan>("ExecutionTime")
                        .HasColumnType("time");

                    b.Property<double?>("FBest")
                        .HasColumnType("float");

                    b.Property<bool>("IsFailed")
                        .HasColumnType("bit");

                    b.Property<int?>("NumberOfEvaluationFitnessFunction")
                        .HasColumnType("int");

                    b.Property<string>("Parameters")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestedAlgorithmId")
                        .HasColumnType("int");

                    b.Property<string>("TestedAlgorithmName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestedFitnessFunctionId")
                        .HasColumnType("int");

                    b.Property<string>("TestedFitnessFunctionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TimerFrequency")
                        .HasColumnType("int");

                    b.Property<string>("XBest")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ExecutedSingleAlgorithms");
                });

            modelBuilder.Entity("MetaheuristicAlgorithmsTester.Domain.Entities.FitnessFunction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfParameters")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FitnessFunctions");
                });

            modelBuilder.Entity("MetaheuristicAlgorithmsTester.Domain.Entities.ParamInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AlgorithmId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFloatingPoint")
                        .HasColumnType("bit");

                    b.Property<double>("LowerBoundary")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("UpperBoundary")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AlgorithmId");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("MetaheuristicAlgorithmsTester.Domain.Entities.ParamInfo", b =>
                {
                    b.HasOne("MetaheuristicAlgorithmsTester.Domain.Entities.Algorithm", null)
                        .WithMany("Parameters")
                        .HasForeignKey("AlgorithmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MetaheuristicAlgorithmsTester.Domain.Entities.Algorithm", b =>
                {
                    b.Navigation("Parameters");
                });
#pragma warning restore 612, 618
        }
    }
}
