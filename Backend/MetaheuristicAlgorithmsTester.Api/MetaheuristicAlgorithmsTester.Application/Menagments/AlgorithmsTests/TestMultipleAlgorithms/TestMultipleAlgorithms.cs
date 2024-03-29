﻿using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithms
{
    public class TestMultipleAlgorithms : IRequest<MultipleAlgorithmTestResult>
    {
        public List<TestAlgorithmDto> Algorithms { get; set; }
        public int FitnessFunctionID { get; set; }
        public int Depth { get; set; }
        public int Dimension { get; set; }
        public double? SatisfiedResult { get; set; }
    }

    public class TestAlgorithmDto
    {
        public int Id { get; set; }
    }
}
