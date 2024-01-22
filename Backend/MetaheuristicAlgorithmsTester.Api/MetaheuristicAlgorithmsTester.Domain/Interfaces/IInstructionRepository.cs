namespace MetaheuristicAlgorithmsTester.Domain.Interfaces
{
    public interface IInstructionRepository
    {
        Task<byte[]> GetInstruction();
        Task<byte[]> GetDll();
    }
}
