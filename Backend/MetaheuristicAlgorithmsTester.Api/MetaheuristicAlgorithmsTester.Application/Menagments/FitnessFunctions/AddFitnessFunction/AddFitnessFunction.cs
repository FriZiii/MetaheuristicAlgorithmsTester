using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.AddFitnessFunction
{
    public class AddFitnessFunction : AddFitnessFunctionDto, IRequest<FitnessFunctionResult>
    {
    }
}
