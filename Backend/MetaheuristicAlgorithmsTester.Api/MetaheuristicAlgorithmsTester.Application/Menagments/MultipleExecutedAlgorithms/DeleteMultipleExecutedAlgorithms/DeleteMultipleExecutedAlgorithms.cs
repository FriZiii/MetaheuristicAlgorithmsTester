using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.MultipleExecutedAlgorithms.DeleteMultipleExecutedAlgorithms
{
    public class DeleteMultipleExecutedAlgorithms : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }
}
