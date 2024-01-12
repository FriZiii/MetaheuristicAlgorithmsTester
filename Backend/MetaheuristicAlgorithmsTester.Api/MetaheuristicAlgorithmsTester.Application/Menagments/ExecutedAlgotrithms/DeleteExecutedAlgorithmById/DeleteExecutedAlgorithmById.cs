using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.ExecutedAlgotrithms.DeleteExecutedAlgorithmById
{
    public class DeleteExecutedAlgorithmById : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
