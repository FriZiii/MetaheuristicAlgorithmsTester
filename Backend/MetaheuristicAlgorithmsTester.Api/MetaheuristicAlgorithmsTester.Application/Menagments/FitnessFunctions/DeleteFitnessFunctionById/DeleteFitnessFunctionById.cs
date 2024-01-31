using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.DeleteFitnessFunctionById
{
    public class DeleteFitnessFunctionById : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
