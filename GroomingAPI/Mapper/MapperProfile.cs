using AutoMapper;
using Grooming.Data.Model;
using Grooming.DTOs;

namespace Grooming.API.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            CreateMap<Pet, PetDTO>()
                .ForMember(dest => dest.AppointmentCount, opt => opt.MapFrom<AppointmentCountResolver>())
                .ReverseMap();
        }
    }
}
