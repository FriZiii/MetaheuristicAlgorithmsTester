using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.SingleExecutedAlgotrithms.DeleteExecutedAlgorithmById
{
    public class DeleteExecutedAlgorithmByIdHandler(IExecutedSingleAlgorithmsRepository executedAlgorithmsRepository) : IRequestHandler<DeleteSingleExecutedAlgorithmById, bool>
    {
        public async Task<bool> Handle(DeleteSingleExecutedAlgorithmById request, CancellationToken cancellationToken)
        {
            return await executedAlgorithmsRepository.DeleteExecutedAlgorithmById(request.Id);
        }
    }
}
