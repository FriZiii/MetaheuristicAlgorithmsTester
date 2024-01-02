using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.GetFitnessFunctionById
{
    public class GetFitnessFunctionById : IRequest<FitnessFunctionResult>
    {
        public int Id { get; set; }
    }
}
