using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.DeleteFitnessFunctionById
{
    public class DeleteFitnessFunctionByIdHandler(IFitnessFunctionRepository fitnessFunctionRepository) : IRequestHandler<DeleteFitnessFunctionById, bool>
    {
        public async Task<bool> Handle(DeleteFitnessFunctionById request, CancellationToken cancellationToken)
        {
            return await fitnessFunctionRepository.DeleteFitnessFunctionById(request.Id);
        }
    }
}
