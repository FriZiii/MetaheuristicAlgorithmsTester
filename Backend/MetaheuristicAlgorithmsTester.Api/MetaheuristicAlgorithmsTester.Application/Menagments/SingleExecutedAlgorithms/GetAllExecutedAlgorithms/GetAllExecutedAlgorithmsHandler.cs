using AutoMapper;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.SingleExecutedAlgotrithms.GetAllExecutedAlgorithms
{
    public class GetAllExecutedAlgorithmsHandler(IExecutedSingleAlgorithmsRepository executedAlgorithmsRepository, IMapper mapper) : IRequestHandler<GetAllSingleExecutedAlgorithms, AllSingleExecutedAlgorithmResult>
    {
        public async Task<AllSingleExecutedAlgorithmResult> Handle(GetAllSingleExecutedAlgorithms request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await executedAlgorithmsRepository.GetAllExecutedAlgorithms();
                if (result != null)
                {
                    return new AllSingleExecutedAlgorithmResult() { IsSuccesfull = true, Message = $"Algorithms have been found", ExecutedAlgorithms = result.Select(x => mapper.Map<SingleExecutedAlgorithmDto>(x)).ToList() };
                }
                else
                {
                    return new AllSingleExecutedAlgorithmResult() { IsSuccesfull = false, Message = $"Algorithms not found" };
                }
            }
            catch (Exception ex)
            {
                return new AllSingleExecutedAlgorithmResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}" };
            }
        }
    }
}
