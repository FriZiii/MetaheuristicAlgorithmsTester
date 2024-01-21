using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.MultipleExecutedAlgorithms.GetAllMultipleExecutedAlgorithms
{
    public class GetAllMultipleExecutedAlgorithmsHandler(IExecutedMultipleAlgorithmsRepository executedMultipleAlgorithmsRepositor) : IRequestHandler<GetAllMultipleExecutedAlgorithms, List<AllMultipleExecutedAlgorithmResult>>
    {
        public async Task<List<AllMultipleExecutedAlgorithmResult>> Handle(GetAllMultipleExecutedAlgorithms request, CancellationToken cancellationToken)
        {
            try
            {
                List<AllMultipleExecutedAlgorithmResult> result = new List<AllMultipleExecutedAlgorithmResult>();
                var multipleExecutedAlgorithm = await executedMultipleAlgorithmsRepositor.GetAllExecutedAlgorithms();

                var prevMultipleExecutedId = string.Empty;
                foreach (var executed in multipleExecutedAlgorithm)
                {
                    if (executed != null)
                    {
                        if (prevMultipleExecutedId != executed.MultipleTestId || prevMultipleExecutedId == string.Empty)
                        {
                            prevMultipleExecutedId = executed.MultipleTestId;
                            result.Add(new AllMultipleExecutedAlgorithmResult()
                            {
                                Date = executed.Date,
                                MultipleTestId = executed.MultipleTestId,
                                ExecutedMultipleAlgorithms = new List<ExecutedMultipleAlgorithms> { executed }
                            });
                        }
                        else
                        {
                            result.First(x => x.MultipleTestId == prevMultipleExecutedId).ExecutedMultipleAlgorithms.Add(executed);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
