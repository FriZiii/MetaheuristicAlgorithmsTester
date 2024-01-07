using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IExecutedAlgorithmsRepository
    {
        public Task AddExecudedAlgorithm(ExecutedAlgorithm executedAlgorithm);
    }
}
