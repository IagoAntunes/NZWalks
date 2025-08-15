using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;

namespace NZWalks.API.Mappings
{
    public class DtoToEntityMapper : Profile
    {
        public DtoToEntityMapper()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Walk,WalkDto>().ReverseMap();
            CreateMap<Difficulty,DifficultyDto>().ReverseMap();
        }
    }
}
