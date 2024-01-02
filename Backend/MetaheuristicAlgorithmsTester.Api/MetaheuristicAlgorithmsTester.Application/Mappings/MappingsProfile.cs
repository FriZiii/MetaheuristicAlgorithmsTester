using AutoMapper;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm;
using MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions;
using MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.AddFitnessFunction;
using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Application.Mappings
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<AddAlgorithmDto, Algorithm>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.DllFile.FileName));

            CreateMap<AddFitnessFunctionDto, FitnessFunction>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.DllFile.FileName));

            CreateMap<Algorithm, AlgorithmDto>();
            CreateMap<FitnessFunction, FitnessFunctionDto>();

            CreateMap<AlgorithmInterfaces.ParamInfo, Domain.Entities.ParamInfo>();
        }
    }
}
