using AlgorithmInterfaces;
using AutoMapper;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using System.Reflection;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm
{
    public class AddAlgorithmHandler : IRequestHandler<AddAlgorithm, AddAlgorithmResult>
    {
        private readonly IMapper mapper;
        private readonly IAlgorithmsRepository algorithmsRepository;

        public AddAlgorithmHandler(IMapper mapper, IAlgorithmsRepository algorithmsRepository)
        {
            this.mapper = mapper;
            this.algorithmsRepository = algorithmsRepository;
        }

        public async Task<AddAlgorithmResult> Handle(AddAlgorithm request, CancellationToken cancellationToken)
        {
            var algorithm = mapper.Map<Algorithm>(request);

            byte[] dllBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                algorithm.DllFile.CopyTo(memoryStream);
                dllBytes = memoryStream.ToArray();
            }

            Assembly assembly = Assembly.Load(dllBytes);
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (typeof(IAlgorithm).IsAssignableFrom(type))
                {
                    var result = await algorithmsRepository.AddAlgorithm(algorithm);
                    if (result != null)
                    {
                        return new AddAlgorithmResult() { IsSuccesfull = true, Message = "The dll file has been added", Algorithm = result };
                    }
                    else
                    {
                        return new AddAlgorithmResult() { IsSuccesfull = false, Message = "Something went wrong" };
                    }
                }
            }

            return new AddAlgorithmResult() { IsSuccesfull = false, Message = "The dll file does not have a class implementing the IAlgorithm interface" };
        }
    }
}