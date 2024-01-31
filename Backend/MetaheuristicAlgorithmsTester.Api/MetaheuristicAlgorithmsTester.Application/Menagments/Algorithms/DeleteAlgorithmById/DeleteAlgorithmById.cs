using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.DeleteAlgorithmById
{
    public class DeleteAlgorithmById : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
