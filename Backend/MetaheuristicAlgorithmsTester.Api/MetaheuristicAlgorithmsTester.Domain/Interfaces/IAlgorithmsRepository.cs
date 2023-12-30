using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IAlgorithmsRepository
    {
        Task AddAlgorithm(Algorithm algorithm);
    }
}
