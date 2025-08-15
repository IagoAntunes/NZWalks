using AutoMapper;
using NZWalks.API.Dtos;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.Domain.Dtos;

namespace NZWalks.API.Mapping
{
    public class RequestToDtoMapper : Profile
    {
        public RequestToDtoMapper()
        {
            CreateMap<Region, AddRegionRequestDto>().ReverseMap();
            CreateMap<WalkDto, AddWalkRequestDto>().ReverseMap();
            //CreateMap<Walk, ImageUploadRequestDto>().ReverseMap();
            CreateMap<LoginDto, LoginRequestDto>().ReverseMap();
            CreateMap<RegisterDto, RegisterRequestDto>().ReverseMap();
            CreateMap<RegionDto, UpdateRegionRequestDto>().ReverseMap();
            CreateMap<WalkDto, UpdateWalkRequestDto>().ReverseMap();

            CreateMap<GetAllWalksFilter, GetAllWalksQueryParameters>().ReverseMap();
        }
    }
}
