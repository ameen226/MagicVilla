using AutoMapper;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDto, CreateVillaDto>().ReverseMap();
            CreateMap<VillaDto, UpdateVillaDto>().ReverseMap();

            CreateMap<VillaNumberDto, CreateVillaNumberDto>().ReverseMap();
            CreateMap<VillaNumberDto, UpdateVillaNumberDto>().ReverseMap();
        }
    }
}
