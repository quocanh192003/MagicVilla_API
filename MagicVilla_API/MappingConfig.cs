using AutoMapper;
using MagicVilla_API.Model;
using MagicVilla_API.Model.dto;

namespace MagicVilla_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();
            CreateMap<Villa, villaCreateDTO>().ReverseMap();
            CreateMap<Villa, villaUpdateDTO>().ReverseMap();
        }
    }
}
