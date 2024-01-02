using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm
{
    public class AddAlgorithm : AddAlgorithmDto, IRequest<AlgorithmResult>
    {
    }
}
