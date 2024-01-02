using AlgorithmInterfaces;
using AutoMapper;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using System.Reflection;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm
{
    public class AddAlgorithmHandler(IMapper mapper, IAlgorithmsRepository algorithmsRepository) : IRequestHandler<AddAlgorithm, AlgorithmResult>
    {
        public async Task<AlgorithmResult> Handle(AddAlgorithm request, CancellationToken cancellationToken)
        {
            try
            {
                var algorithm = mapper.Map<Algorithm>(request);

                byte[] dllBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    request.DllFile.CopyTo(memoryStream);
                    dllBytes = memoryStream.ToArray();
                    algorithm.DllFileBytes = dllBytes;
                }

                Assembly assembly = Assembly.Load(dllBytes);
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (typeof(IAlgorithm).IsAssignableFrom(type))
                    {
                        PropertyInfo paramsInfoProperty = type.GetProperty("ParamsInfo");
                        if (paramsInfoProperty != null)
                        {
                            object algorithmInstance = Activator.CreateInstance(type);

                            List<AlgorithmInterfaces.ParamInfo> paramsInfoList = (paramsInfoProperty.GetValue(algorithmInstance) as AlgorithmInterfaces.ParamInfo[]).ToList();


                            algorithm.Parameters = paramsInfoList.Select(mapper.Map<Domain.Entities.ParamInfo>).ToList();

                            var result = await algorithmsRepository.AddAlgorithm(algorithm);
                            if (result != null)
                            {
                                return new AlgorithmResult() { IsSuccesfull = true, Message = "The dll file has been added", Algorithm = mapper.Map<AlgorithmDto>(result) };
                            }
                            else
                            {
                                return new AlgorithmResult() { IsSuccesfull = false, Message = "Something went wrong" };
                            }
                        }
                        else
                        {
                            return new AlgorithmResult() { IsSuccesfull = false, Message = "The Algorithm does not have a ParamInfo" };
                        }
                    }
                }

                return new AlgorithmResult() { IsSuccesfull = false, Message = "The dll file does not have a class implementing the IAlgorithm interface" };
            }
            catch (Exception ex)
            {
                return new AlgorithmResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}" };
            }
        }
    }
}