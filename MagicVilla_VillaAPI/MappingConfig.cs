using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {

        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa, CreateVillaDto>().ReverseMap();
            CreateMap<Villa, UpdateVillaDto>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, CreateVillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, UpdateVillaNumberDto>().ReverseMap();

            CreateMap<ApplicationUser, UserDto>().ReverseMap();

        }

    }
}
