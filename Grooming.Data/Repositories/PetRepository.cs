using Grooming.Data.Context;
using Grooming.Data.Filters;
using Grooming.Data.Interfaces;
using Grooming.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grooming.Data.Repositories
{
    /// <summary>
    /// This class handles methods for making requests to the pet repository.
    /// </summary>
    public class PetRepository : IPetRepository
    {
        private readonly IPetCtx _ctx;

        public PetRepository(IPetCtx ctx)
        {
            _ctx = ctx;
        }

        public async Task<Pet> GetPetByIdAsync(int petId)
        {
            return await _ctx.Pets
                .AsNoTracking()
                .WherePetIdEquals(petId)
                .Include(pet => pet.Appointments)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsAsync()
        {
            return await _ctx.Pets
                .AsNoTracking()
                .Include(pet => pet.Appointments)
                .ToListAsync();
        }

        public async Task<Pet> DeletePetByIdAsync(int petId)
        {
            var pet = await _ctx.Pets.AsNoTracking().WherePetIdEquals(petId).SingleOrDefaultAsync();
            _ctx.Pets.Remove(pet);
            await _ctx.SaveChangesAsync();
            return pet;
        }

        public async Task<List<Appointment>> GetAppointmentsByPetIdAsync(int petId)
        {
            return await _ctx.Appointments
                .AsNoTracking()
                .WherePetIdForAppointmentEquals(petId)
                .ToListAsync();
        }

        public async Task<Pet> UpdatePetByIdAsync(Pet newPet, int petId)
        {
            var pet = await _ctx.Pets.AsNoTracking().WherePetIdEquals(petId).SingleOrDefaultAsync();

            pet.Id = pet.Id;

            if (newPet.PetName != null)
            {
                pet.PetName = newPet.PetName;
            }
            else
            {
                pet.PetName = pet.PetName;
            }
            if (newPet.Age != 0)
            {
                pet.Age = newPet.Age;
            }
            else
            {
                pet.Age = pet.Age;
            }
            if (newPet.Weight != 0)
            {
                pet.Weight = newPet.Weight;
            }
            else
            {
                pet.Weight = pet.Weight;
            }
            if (newPet.Species != null)
            {
                pet.Species = newPet.Species;
            }
            else
            {
                pet.Species = pet.Species;
            }

            if (newPet.Breed != null)
            {
                pet.Breed = newPet.Breed;
            }
            else
            {
                pet.Breed = pet.Breed;
            }

            if (newPet.Fur != null)
            {
                pet.Fur = newPet.Fur;
            }
            else
            {
                pet.Fur = pet.Fur;
            }
            if(newPet.OwnerName != null)
            {
                pet.OwnerName = newPet.OwnerName;
            }
            else
            {
                pet.OwnerName = pet.OwnerName;
            }

            if (newPet.OwnerEmail != null)
            {
                pet.OwnerEmail = newPet.OwnerEmail;
            }
            else
            {
                pet.OwnerEmail = pet.OwnerEmail;
            }

            if (newPet.OwnerPhone != null)
            {
                pet.OwnerPhone = newPet.OwnerPhone;
            }
            else
            {
                pet.OwnerPhone = pet.OwnerPhone;
            }

            _ctx.Pets.Update(pet);
            await _ctx.SaveChangesAsync();
            return pet;
        }

        public async Task<Pet> CreatePetAsync(Pet pet)
        {
            _ctx.Pets.Add(pet);
            await _ctx.SaveChangesAsync();

            return pet;
        }
    }
}
