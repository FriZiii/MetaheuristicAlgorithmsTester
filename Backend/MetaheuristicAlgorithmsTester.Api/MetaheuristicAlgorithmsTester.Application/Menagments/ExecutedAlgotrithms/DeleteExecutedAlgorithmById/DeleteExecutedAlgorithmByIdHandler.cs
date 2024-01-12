using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.ExecutedAlgotrithms.DeleteExecutedAlgorithmById
{
    public class DeleteExecutedAlgorithmByIdHandler(IExecutedAlgorithmsRepository executedAlgorithmsRepository) : IRequestHandler<DeleteExecutedAlgorithmById, bool>
    {
        public async Task<bool> Handle(DeleteExecutedAlgorithmById request, CancellationToken cancellationToken)
        {
            return await executedAlgorithmsRepository.DeleteExecutedAlgorithmById(request.Id);
        }
    }
}
