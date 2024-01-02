using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.GetAlgorithmById
{
    public class GetAlgorithmById : IRequest<AlgorithmResult>
    {
        public int Id { get; set; }
    }
}
