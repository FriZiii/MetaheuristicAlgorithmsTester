using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.SingleExecutedAlgotrithms.DeleteExecutedAlgorithmById
{
    public class DeleteSingleExecutedAlgorithmById : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
