using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IExecutedAlgorithmsRepository
    {
        Task UpdateExecutedAlgorithm(int id, ExecutedAlgorithm updatedExecutedAlgorithm);
        Task<int> AddExecudedAlgorithm(ExecutedAlgorithm executedAlgorithm);
        Task<ExecutedAlgorithm?> GetExecutedAlgorithmById(int algorithmId);
        Task<bool> DeleteExecutedAlgorithmById(int id);
        Task<IEnumerable<ExecutedAlgorithm?>> GetAllExecutedAlgorithms();
        Task<int> GetCurrentExecutedId();
    }
}
