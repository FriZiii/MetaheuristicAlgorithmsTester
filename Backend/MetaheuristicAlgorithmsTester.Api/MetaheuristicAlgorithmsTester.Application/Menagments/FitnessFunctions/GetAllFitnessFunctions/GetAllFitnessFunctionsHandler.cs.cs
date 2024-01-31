using AutoMapper;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.GetAllFitnessFunctions
{
    public class GetAllFitnessFunctionsHandler(IMapper mapper, IFitnessFunctionRepository fitnessFunctionRepository) : IRequestHandler<GetAllFitnessFunctions, AllFitnessFunctionsResult>
    {
        public async Task<AllFitnessFunctionsResult> Handle(GetAllFitnessFunctions request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await fitnessFunctionRepository.GetAllFitnessFunctions();
                if (result != null)
                {

                    return new AllFitnessFunctionsResult() { IsSuccesfull = true, Message = $"Fitness functions have been found", FitnessFunctions = result.Select(x => mapper.Map<FitnessFunctionDto>(x)).ToList() };
                }
                else
                {
                    return new AllFitnessFunctionsResult() { IsSuccesfull = false, Message = $"Fitness functions not found" };
                }
            }
            catch (Exception ex)
            {
                return new AllFitnessFunctionsResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}" };
            }
        }
    }
}
