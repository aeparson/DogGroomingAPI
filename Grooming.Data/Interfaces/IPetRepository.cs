using System.Collections.Generic;
using System.Threading.Tasks;
using Grooming.Data.Model;

namespace Grooming.Data.Interfaces
{
    /// <summary>
    /// This interface provides an abstraction layer for pet repository methods.
    /// </summary>
    /// 
    public interface IPetRepository
    {
        Task<Pet> GetPetByIdAsync(int petId);

        Task<IEnumerable<Pet>> GetPetsAsync();

        Task<Pet> DeletePetByIdAsync(int petId);

        Task<Pet> UpdatePetByIdAsync(Pet newPet, int petId);

        Task<Pet> CreatePetAsync(Pet newPet);

        Task<List<Appointment>> GetAppointmentsByPetIdAsync(int petId);



    }
}
