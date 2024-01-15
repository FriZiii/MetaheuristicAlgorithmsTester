namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IAlgorithmStateRepository
    {
        Task SaveState(string state, string fileName);
        Task<string> GetState(int executedId);
    }
}
