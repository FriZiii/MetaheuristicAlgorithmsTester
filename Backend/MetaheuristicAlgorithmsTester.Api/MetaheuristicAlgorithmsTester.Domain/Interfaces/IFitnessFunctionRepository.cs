using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IFitnessFunctionRepository
    {
        Task<FitnessFunction?> AddFitnessFunction(FitnessFunction fitnessFunction);
        Task<FitnessFunction?> GetFitnessFunctionById(int id);
        Task<IEnumerable<FitnessFunction?>> GetAllFitnessFunctions();
    }
}
