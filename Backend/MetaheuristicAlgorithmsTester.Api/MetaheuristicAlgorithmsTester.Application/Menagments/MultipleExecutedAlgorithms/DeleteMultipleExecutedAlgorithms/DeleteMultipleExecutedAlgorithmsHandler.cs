using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.MultipleExecutedAlgorithms.DeleteMultipleExecutedAlgorithms
{
    public class DeleteMultipleExecutedAlgorithmsHandler(IExecutedMultipleAlgorithmsRepositor executedMultipleAlgorithmsRepositor) : IRequestHandler<DeleteMultipleExecutedAlgorithms, bool>
    {
        public async Task<bool> Handle(DeleteMultipleExecutedAlgorithms request, CancellationToken cancellationToken)
        {
            return await executedMultipleAlgorithmsRepositor.DeleteExecutedAlgorithmById(request.Id);
        }
    }
}
