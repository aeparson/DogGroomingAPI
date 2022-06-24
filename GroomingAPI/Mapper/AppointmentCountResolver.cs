using System.Linq;
using AutoMapper;
using Grooming.Data.Model;
using Grooming.DTOs;

namespace Grooming.API.Mapper
{
    public class AppointmentCountResolver : IValueResolver<Pet, PetDTO, int>
    {
        public int Resolve(Pet pet, PetDTO petDTO, int destMember, ResolutionContext context)
        {
            if (pet.Appointments == null)
            {
                return 0;
            }
            return pet.Appointments.Count();
        }
    }
}
