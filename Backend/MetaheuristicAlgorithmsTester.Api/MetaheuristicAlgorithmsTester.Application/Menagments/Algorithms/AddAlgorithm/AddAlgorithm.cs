using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm
{
    public class AddAlgorithm : AddAlgorithmDto, IRequest<Algorithm>
    {
    }
}
