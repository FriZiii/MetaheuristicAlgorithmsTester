using AutoMapper;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm;
using MetaheuristicAlgorithmsTester.Application.Menagments.SingleExecutedAlgotrithms;
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
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => $"{src.Name}-{DateTime.Now:yy-MM-dd_HH-mm-ss}"));
            CreateMap<Algorithm, AlgorithmDto>();

            CreateMap<AddFitnessFunctionDto, FitnessFunction>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => $"{src.Name}-{DateTime.Now:yy-MM-dd_HH-mm-ss}"));

            CreateMap<FitnessFunction, FitnessFunctionDto>();

            CreateMap<AlgorithmInterfaces.ParamInfo, Domain.Entities.ParamInfo>();

            CreateMap<ExecutedSingleAlgorithm, SingleExecutedAlgorithmDto>();
        }
    }
}
