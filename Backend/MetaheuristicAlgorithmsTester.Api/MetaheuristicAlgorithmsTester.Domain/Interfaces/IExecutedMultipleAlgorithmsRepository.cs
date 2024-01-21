using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IExecutedMultipleAlgorithmsRepository
    {
        Task UpdateExecutedAlgorithm(int id, ExecutedMultipleAlgorithms updatedExecutedAlgorithm);
        Task<int> AddExecudedAlgorithm(ExecutedMultipleAlgorithms executedAlgorithm);
        Task<List<ExecutedMultipleAlgorithms?>> GetExecutedAlgorithmsByExecutedId(string executedId);
        Task<bool> DeleteExecutedAlgorithmById(string id);
        Task<ExecutedMultipleAlgorithms?> GetExecutedAlgorithmById(int id);
        Task<IEnumerable<ExecutedMultipleAlgorithms?>> GetAllExecutedAlgorithms();
    }
}
