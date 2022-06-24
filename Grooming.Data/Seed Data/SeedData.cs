using Grooming.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Grooming.Data.SeedData
{
    public static class Extensions
    {
        /// <summary>
        /// Produces a set of seed data to insert into the database on startup.
        /// </summary>
        /// <param name="modelBuilder">Used to build model base DbContext.</param>
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            var petFactory = new PetFactory();
            var pets = petFactory.GenerateRandomPets(20);

            modelBuilder.Entity<Pet>().HasData(pets);

            var appointmentFactory = new AppointmentFactory();
            var appointments = appointmentFactory.GenerateRandomAppointments(40);


            modelBuilder.Entity<Appointment>().HasData(appointments);

        }
    }
}
