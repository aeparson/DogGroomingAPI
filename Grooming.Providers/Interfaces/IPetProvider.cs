using Grooming.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grooming.Providers.Interfaces
{
    /// <summary>
    /// This interface provides an abstraction layer for pet related service methods.
    /// </summary>
    public interface IPetProvider
    {
        Task<Pet> GetPetByIdAsync(int petId);

        Task<Pet> CreatePetAsync(Pet pet);

#nullable enable
        Task<IEnumerable<Pet>> GetPetsAsync();

        Task<Pet> DeletePetByIdAsync(int petId);

        Task<Pet> UpdatePetByIdAsync(Pet newPet, int petId);

        Task<List<Appointment>> GetAppointmentsByPetIdAsync(int petId);
    }
}
