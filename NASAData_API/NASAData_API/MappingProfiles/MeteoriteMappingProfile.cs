using AutoMapper;
using NASAData_API.DbModels;
using NASAData_API.Models;

namespace NASAData_API.MappingProfiles
{
    public class MeteoriteMappingProfile : Profile
    {
        public MeteoriteMappingProfile()
        {
            CreateMap<Meteorite, DbMeteorite>()
            .ForMember(opt => opt.Lat,
                       inp => inp.MapFrom(src => src.Geolocation.Coordinates.LastOrDefault()))
            .ForMember(opt => opt.Lon,
                       inp => inp.MapFrom(src => src.Geolocation.Coordinates.FirstOrDefault()))
            .ForMember(opt => opt.Type,
                       inp => inp.MapFrom(src => src.Geolocation.Type));
        }
    }
}


