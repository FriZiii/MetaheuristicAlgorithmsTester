namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IAlgorithmStateRepository
    {
        Task SaveState(string state, string fileName);
        Task<string> GetStateOfSingleTest(int executedId);
        Task<string> GetStateOfMultipleTest(int executedId);
    }
}
