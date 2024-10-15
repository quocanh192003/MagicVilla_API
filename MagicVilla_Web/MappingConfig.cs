using AutoMapper;
using MagicVilla_Web.Model.dto;


namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDTO, villaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, villaUpdateDTO>().ReverseMap();
            
            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
        }
    }
}
