using AutoMapper;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Profiles
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<Plate, PlateDto>();
            CreateMap<PlateForCreationDto, Plate>();
            CreateMap<PlateForUpdateDto, Plate>();
            CreateMap<PlateForEndWorkDto, Plate>();
        }
    }
}