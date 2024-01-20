using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IExecutedSingleAlgorithmsRepository
    {
        Task UpdateExecutedAlgorithm(int id, ExecutedSingleAlgorithm updatedExecutedAlgorithm);
        Task<int> AddExecudedAlgorithm(ExecutedSingleAlgorithm executedAlgorithm);
        Task<ExecutedSingleAlgorithm?> GetExecutedAlgorithmById(int algorithmId);
        Task<bool> DeleteExecutedAlgorithmById(int id);
        Task<IEnumerable<ExecutedSingleAlgorithm?>> GetAllExecutedAlgorithms();
    }
}
