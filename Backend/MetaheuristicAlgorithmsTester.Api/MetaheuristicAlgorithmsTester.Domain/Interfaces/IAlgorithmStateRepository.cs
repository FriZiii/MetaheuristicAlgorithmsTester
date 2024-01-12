namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IAlgorithmStateRepository
    {
        Task SaveState(string state, int executedId, string fileName);
        Task<string> GetState(int executedId);
    }
}
