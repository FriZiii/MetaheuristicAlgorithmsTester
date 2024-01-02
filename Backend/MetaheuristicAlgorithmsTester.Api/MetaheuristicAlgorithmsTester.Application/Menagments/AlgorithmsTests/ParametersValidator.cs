
using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests
{
    public static class ParametersValidator
    {
        public static List<ParametersError> Validate(List<ParamInfo> paramInfos, List<double> parameters)
        {
            List<ParametersError> errors = new List<ParametersError>();

            if (paramInfos.Count != parameters.Count)
            {
                errors.Add(new ParametersError() { Message = $"The number of parameters should be {paramInfos.Count}" });
                return errors;
            }
            else
            {
                for (int i = 0; i < paramInfos.Count; i++)
                {
                    if (parameters[i] < paramInfos[i].LowerBoundary)
                    {
                        errors.Add(new ParametersError() { Message = $"The parameter {paramInfos[i].Name} should be greater then {paramInfos[i].LowerBoundary}", ParameterId = paramInfos[i].Id });
                    }
                    if (parameters[i] > paramInfos[i].UpperBoundary)
                    {
                        errors.Add(new ParametersError() { Message = $"The parameter {paramInfos[i].Name} should be less then {paramInfos[i].UpperBoundary}", ParameterId = paramInfos[i].Id });
                    }
                }
                return errors;
            }
        }
    }

    public class ParametersError
    {
        public int ParameterId { get; set; }
        public string Message { get; set; } = default!;
    }
}
