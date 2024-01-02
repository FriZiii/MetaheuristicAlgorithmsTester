using AlgorithmInterfaces;
using AutoMapper;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using System.Reflection;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.AddFitnessFunction
{
    public class AddFitnessFunctionHandler(IMapper mapper, IFitnessFunctionRepository fitnessFunctionRepository) : IRequestHandler<AddFitnessFunction, FitnessFunctionResult>
    {
        public async Task<FitnessFunctionResult> Handle(AddFitnessFunction request, CancellationToken cancellationToken)
        {
            try
            {
                var fitnessFunction = mapper.Map<FitnessFunction>(request);

                byte[] dllBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    request.DllFile.CopyTo(memoryStream);
                    dllBytes = memoryStream.ToArray();
                    fitnessFunction.DllFileBytes = dllBytes;
                }

                Assembly assembly = Assembly.Load(dllBytes);
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (typeof(IFitnessFunction).IsAssignableFrom(type))
                    {
                        var result = await fitnessFunctionRepository.AddFitnessFunction(fitnessFunction);
                        if (result != null)
                        {
                            return new FitnessFunctionResult() { IsSuccesfull = true, Message = "The dll file has been added", FitnessFunction = mapper.Map<FitnessFunctionDto>(result) };
                        }
                        else
                        {
                            return new FitnessFunctionResult() { IsSuccesfull = false, Message = "Something went wrong" };
                        }
                    }
                }
                return new FitnessFunctionResult() { IsSuccesfull = false, Message = "The dll file does not have a class implementing the IFitnessFunction interface" };
            }
            catch (Exception ex)
            {
                return new FitnessFunctionResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}" };
            }
        }
    }
}
