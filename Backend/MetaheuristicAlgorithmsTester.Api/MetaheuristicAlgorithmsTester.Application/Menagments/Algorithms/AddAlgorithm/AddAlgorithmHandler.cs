using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm
{
    public class AddAlgorithmHandler : IRequestHandler<AddAlgorithm, Algorithm>
    {
        private readonly IAlgorithmsRepository algorithmsRepository;

        public AddAlgorithmHandler(IAlgorithmsRepository algorithmsRepository)
        {
            this.algorithmsRepository = algorithmsRepository;
        }

        public async Task<Algorithm> Handle(AddAlgorithm request, CancellationToken cancellationToken)
        {
            var algorithm = new Algorithm();

            await algorithmsRepository.AddAlgorithm(algorithm);
            return algorithm;
        }
    }
}
