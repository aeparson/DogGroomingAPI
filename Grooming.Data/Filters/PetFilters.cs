using System;
using System.Collections.Generic;
using System.Linq;
using Grooming.Data.Model;

namespace Grooming.Data.Filters
{
    /// <summary>
    /// Filter collection for pet and appointment context queries.
    /// </summary>
    public static class PetFilters
    {
        /// <summary>
        /// Returns the pet with a given Id.
        /// </summary>
        /// <param name="pets"></param>
        /// <param name="petId"></param>
        /// <returns>The matching pet.</returns>
        public static IQueryable<Pet> WherePetIdEquals(this IQueryable<Pet> pets, int petId)
        {
            return pets.Where(p => p.Id == petId).AsQueryable();
        }

        /// <summary>
        /// Returns the appointments for a pet with a given Id.
        /// </summary>
        /// <param name="appointments"></param>
        /// <param name="petId"></param>
        /// <returns>The matching pet.</returns>
        public static IQueryable<Appointment> WherePetIdForAppointmentEquals(this IQueryable<Appointment> appointments, int petId)
        {
            return appointments.Where(p => p.PetId == petId).AsQueryable();
        }

        /// <summary>
        /// Returns the appointment with a given Id.
        /// </summary>
        /// <param name="appointments"></param>
        /// <param name="appointmentId"></param>
        /// <returns>The matching appointment.</returns>
        public static IQueryable<Appointment> WhereAppointmentIdEquals(this IQueryable<Appointment> appointments, int appointmentId)
        {
            return appointments.Where(p => p.Id == appointmentId).AsQueryable();
        }
    }
}
