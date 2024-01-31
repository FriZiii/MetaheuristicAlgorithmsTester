using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Instruction.GetInstruction
{
    public class GetInstructionHandler(IInstructionRepository instructionRepository) : IRequestHandler<GetInstruction, InstructionResult>
    {
        public async Task<InstructionResult> Handle(GetInstruction request, CancellationToken cancellationToken)
        {
            var content = await instructionRepository.GetInstruction();
            return new InstructionResult()
            {
                FileContent = content,
                FileName = "Instruction.pdf",
                ContentType = "application/octet-stream"
            };
        }
    }
}
