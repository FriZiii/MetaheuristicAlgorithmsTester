using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IExecutedAlgorithmsRepository
    {
        public Task<int> AddExecudedAlgorithm(ExecutedAlgorithm executedAlgorithm);
        Task<ExecutedAlgorithm?> GetExecutedAlgorithmById(int algorithmId);
    }
}
