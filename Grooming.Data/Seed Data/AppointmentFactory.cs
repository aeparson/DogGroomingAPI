using Grooming.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grooming.Data.SeedData
{
    /// <summary>
    /// This class provides tools for generating random appointment objects.
    /// </summary>
    public class AppointmentFactory
    {
        readonly Random _rand = new();

        public AppointmentFactory()
        {
        }

        /// <summary>
        /// Generates a number of random appointment objects based on input.
        /// </summary>
        /// <param name="numberOfAppointments">The number of random appointments to generate.</param>
        /// <returns>A list of random appointments.</returns>
        public List<Appointment> GenerateRandomAppointments(int numberOfAppointments)
        {
            var AppointmentList = new List<Appointment>();

            for (var i = 0; i < numberOfAppointments; i++)
            {
                AppointmentList.Add(CreateRandomAppointment(i + 1));
            }

            return AppointmentList;
        }

        public readonly List<string> _shampoos = new()
        {
            "EasiDri",
            "Lillidale Medicated",
            "Lillidale Colour Enhancing",
            "Animology Curly Coat",
            "Lillidale Tea Tree",
            "WildWash Beauty and Shine",
            "Animology Deep Clean",
            "Animology Puppy Love",
            "Best Shot UltraMax",
            "Wahl Mucky Puppy"

        };

        public readonly List<string> _conditioners = new()
        {
            "Shires Digby & Fox Protein",
            "WildWash",
            "Animology Top Dog",
            "Best Shot UltraMax",
            "Wahl Mucky Puppy"

        };

        public readonly List<string> _groomer = new()
        {
            "Mandy",
            "Jeff",
            "Kirsten",
            "Peyton",
            "Arcadia"

        };

        public readonly List<string> _bathMethods = new()
        {
            "Bath & Brush",
            "Bath & Brush with FURminator",
            "Bath & Full Haircut",
            "Bath & Full Haircut with FURminator",
            "Puppy Bath & Brush",
            "Puppy Bath & Trim",
            "Pubby Bath & Full Haircut"
        };

        

        public readonly List<string> _brushInfo = new()
        {
            "FURminator",
            "Slicker",
            "Pinhead",
            "Bristle",
            "Stripping Comb",
        };

        public readonly List<string> _behaviorNotes = new()
        {
            "Super sweet baby, very calm.",
            "Don't forget the calming snood!",
            "Doesn't like other dogs, but is fine with cats around.",
            "Bites during nail trims.",
            "Will try to leap off the table at any opportunity.",
            "Hates baths, but LOVES being blowdried. Will scratch during baths.",
        };

        public readonly List<string> _additionalServices = new()
        {
            "Ear Cleaning",
            "Nail Trim",
            "Fur Styling",
            "Nail Painting",
            "Dye Patch",
        };

        public readonly List<string> _ownerDebrief = new()
        {
            "Suggested she bring him in more often so he doesn't get so matted.",
            "Brushing a few times a week will cut down on matting.",
            "Nails need to be trimmed more than twice a year.",
            "Really loves the blowdrier.",
            "Will be out of town for a few months, pet sitter will be doing drop off/pick up.",
            "Will be getting a new puppy so next appointment is for two.",
        };

        public readonly List<string> _notes = new()
        {
            "Is usually matted, schedule extra time.",
            "Make sure to bring the muzzle.",
            "Is afraid of the dremel, use clippers for nails.",
            "So sweet, but wants cuddles before AND after bath.",
            "Really thick undercoat.",
            "Has caps on claws.",
        };


        /// <summary>
        /// Uses random generators to build appointment entries.
        /// </summary>
        /// <param name="id">ID to assign to the appointment.</param>
        /// <returns>A randomly generated appointment file.</returns>
        public Appointment CreateRandomAppointment(int id)
        {

            Appointment appointment = new()
            {
                Id = id,
                PetId = _rand.Next(1, 20),
                Shampoos = _shampoos[_rand.Next(0, 10)],
                Conditioners = _conditioners[_rand.Next(0, 5)],
                Groomer = _groomer[_rand.Next(0, 5)],
                BathMethods = _bathMethods[_rand.Next(0, 7)],
                BrushInfo = _brushInfo[_rand.Next(0, 5)],
                BehaviorNotes = _behaviorNotes[_rand.Next(0, 6)],
                AdditionalServices = _additionalServices[_rand.Next(0, 5)],
                OwnerDebrief = _ownerDebrief[_rand.Next(0, 6)],
                Notes = _notes[_rand.Next(0, 6)],
                TotalCost = (decimal)Math.Round(_rand.NextDouble() * ((100 - 0) + 1), 2),
                Date = RandomDay(),

            };
            return appointment;
        }

        /// <summary>
        /// Generates a random date between Jan 1, 2022 and Dec 31, 2050.
        /// </summary>
        /// <returns>A random date between Jan 1, 2030 and Dec 31, 2050.</returns>
        private Random gen = new();
        private string RandomDay()
        {
            DateTime start = new DateTime(2022, 1, 1);
            DateTime end = new DateTime(2030, 12, 31);
            int range = (end - start).Days;
            return start.AddDays(gen.Next(range)).ToString("yyyy-MM-dd");
        }
    }
}
