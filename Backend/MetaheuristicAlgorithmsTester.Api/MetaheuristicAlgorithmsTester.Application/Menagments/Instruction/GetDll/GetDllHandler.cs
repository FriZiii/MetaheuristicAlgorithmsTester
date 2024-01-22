using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Instruction.GetDll
{
    public class GetDllHandler(IInstructionRepository instructionRepository) : IRequestHandler<GetDll, InstructionResult>
    {
        public async Task<InstructionResult> Handle(GetDll request, CancellationToken cancellationToken)
        {
            var content = await instructionRepository.GetDll();
            return new InstructionResult()
            {
                FileContent = content,
                FileName = "AlgorithmInterfaces.dll",
                ContentType = "application/octet-stream"
            };
        }
    }
}
