using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IExecutedMultipleAlgorithmsRepositor
    {
        Task UpdateExecutedAlgorithm(int id, ExecutedMultipleAlgorithms updatedExecutedAlgorithm);
        Task<int> AddExecudedAlgorithm(ExecutedMultipleAlgorithms executedAlgorithm);
        Task<List<ExecutedMultipleAlgorithms?>> GetExecutedAlgorithmsByExecutedId(string executedId);
        Task<bool> DeleteExecutedAlgorithmById(string id);
        Task<IEnumerable<ExecutedMultipleAlgorithms?>> GetAllExecutedAlgorithms();
    }
}
