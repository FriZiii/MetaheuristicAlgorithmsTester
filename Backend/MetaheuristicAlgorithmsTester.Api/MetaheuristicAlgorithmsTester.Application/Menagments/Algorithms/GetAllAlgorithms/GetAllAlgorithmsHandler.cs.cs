using AutoMapper;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.GetAllAlgorithms
{
    public class GetAllAlgorithmsHandler(IMapper mapper, IAlgorithmsRepository algorithmRepository) : IRequestHandler<GetAllAlgorithms, AllAlgorithmsResult>
    {
        public async Task<AllAlgorithmsResult> Handle(GetAllAlgorithms request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await algorithmRepository.GetAllAlgorithms();
                if (result != null)
                {

                    return new AllAlgorithmsResult() { IsSuccesfull = true, Message = $"Algorithms have been found", Algorithms = result.Select(x => mapper.Map<AlgorithmDto>(x)).ToList() };
                }
                else
                {
                    return new AllAlgorithmsResult() { IsSuccesfull = false, Message = $"Algorithms not found" };
                }
            }
            catch (Exception ex)
            {
                return new AllAlgorithmsResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}" };
            }
        }
    }
}
