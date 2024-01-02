using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IAlgorithmsRepository
    {
        Task<Algorithm?> AddAlgorithm(Algorithm algorithm);
        Task<Algorithm?> GetAlgorithm(int id);
    }
}
