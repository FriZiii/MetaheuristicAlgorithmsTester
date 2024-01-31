using AutoMapper;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.GetFitnessFunctionById
{
    public class GetFitnessFunctionByIdHandler(IMapper mapper, IFitnessFunctionRepository fitnessFunctionRepository) : IRequestHandler<GetFitnessFunctionById, FitnessFunctionResult>
    {
        public async Task<FitnessFunctionResult> Handle(GetFitnessFunctionById request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await fitnessFunctionRepository.GetFitnessFunctionById(request.Id);

                if (result != null)
                {
                    return new FitnessFunctionResult() { IsSuccesfull = true, Message = $"The fitness function with id {request.Id} was found", FitnessFunction = mapper.Map<FitnessFunctionDto>(result) };
                }
                else
                {
                    return new FitnessFunctionResult() { IsSuccesfull = false, Message = $"No fitness function found with id equal to {request.Id}" };
                }
            }
            catch (Exception ex)
            {
                return new FitnessFunctionResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}" };
            }
        }
    }
}
