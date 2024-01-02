using AutoMapper;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm;
using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Application.Mappings
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<AddAlgorithmDto, Algorithm>();
        }
    }
}
