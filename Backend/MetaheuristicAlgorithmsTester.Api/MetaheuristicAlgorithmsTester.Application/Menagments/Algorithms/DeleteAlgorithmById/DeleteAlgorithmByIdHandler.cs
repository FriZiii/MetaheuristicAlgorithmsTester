using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.DeleteAlgorithmById
{
    public class DeleteAlgorithmByIdHandler(IAlgorithmsRepository algorithmsRepository) : IRequestHandler<DeleteAlgorithmById, bool>
    {
        public async Task<bool> Handle(DeleteAlgorithmById request, CancellationToken cancellationToken)
        {
            return await algorithmsRepository.DeleteAlgorithmById(request.Id);
        }
    }
}
