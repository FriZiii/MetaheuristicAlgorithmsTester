using AutoMapper;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.ExecutedAlgotrithms.GetAllExecutedAlgorithms
{
    public class GetAllExecutedAlgorithmsHandler(IExecutedAlgorithmsRepository executedAlgorithmsRepository, IMapper mapper) : IRequestHandler<GetAllExecutedAlgorithms, AllExecutedAlgorithmResult>
    {
        public async Task<AllExecutedAlgorithmResult> Handle(GetAllExecutedAlgorithms request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await executedAlgorithmsRepository.GetAllExecutedAlgorithms();
                if (result != null)
                {
                    return new AllExecutedAlgorithmResult() { IsSuccesfull = true, Message = $"Algorithms have been found", ExecutedAlgorithms = result.Select(x => mapper.Map<ExecutedAlgorithmDto>(x)).ToList() };
                }
                else
                {
                    return new AllExecutedAlgorithmResult() { IsSuccesfull = false, Message = $"Algorithms not found" };
                }
            }
            catch (Exception ex)
            {
                return new AllExecutedAlgorithmResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}" };
            }
        }
    }
}
