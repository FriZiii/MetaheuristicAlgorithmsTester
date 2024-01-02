using AutoMapper;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.GetAlgorithmById
{
    public class GetAlgorithmByIdHandler(IMapper mapper, IAlgorithmsRepository algorithmsRepository) : IRequestHandler<GetAlgorithmById, AlgorithmResult>
    {
        public async Task<AlgorithmResult> Handle(GetAlgorithmById request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await algorithmsRepository.GetAlgorithmById(request.Id);

                if (result != null)
                {
                    return new AlgorithmResult() { IsSuccesfull = true, Message = $"The algorithm with id {request.Id} was found", Algorithm = mapper.Map<AlgorithmDto>(result) };
                }
                else
                {
                    return new AlgorithmResult() { IsSuccesfull = false, Message = $"No algorithm found with id equal to {request.Id}" };
                }
            }
            catch (Exception ex)
            {
                return new AlgorithmResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}" };
            }
        }
    }
}
