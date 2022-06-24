using Grooming.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grooming.Data.SeedData
{
    /// <summary>
    /// This class provides tools for generating random pets.
    /// </summary>
    public class PetFactory
    {
        readonly Random _rand = new();

        public readonly List<string> _petName = new()
        {
            "Bruce",
            "Stella",
            "Biscuits",
            "Dante",
            "Freja",
            "Leila",
            "Norbert",
            "Rupert",
            "Alice",
            "Minerva",
            "David"
        };

        public readonly List<string> _species = new()
        {
            "Dog",
            "Cat",
            "Lizard"
        };

        public readonly List<string> _breed = new()
        {
            "Pitbull",
            "Dachshund",
            "Papillion",
            "Golden Retriever",
            "Persian",
            "Sphynx",
            "Siamese",
            "Cavalier",
            "Corgi",
            "Dalmation",
            "Lab"
        };

        public readonly List<string> _fur = new()
        {
            "Long Coat",
            "Short Coat",
            "Double Coat",
            "Curly Coat",
            "Wire Coat",
            "Silky Coat",
            "Hairless",
            "Medium Coat",
            "Coarse Coat"
        };

        public readonly List<string> _ownerName = new()
        {
            "Spok",
            "Steve",
            "Cassie",
            "John",
            "Shawn",
            "Cody",
            "Barb",
            "Chuck",
            "Meagan",
            "Ruth",
            "Katrina"
        };
        public readonly List<string> _ownerEmail1 = new()
        {
            "FurbyLover",
            "BakedFrogLegs",
            "CreepyStranger",
            "SnootBooper",
            "GoogleStoleMyIdea",
            "DangerousWithRocks",
            "BooniePigFriend",
            "MeForPresident",
            "TinfoilHatSociety",
            "HairyPoppins",
            "HogwartsDropout",
            "BigfootNumber1Fan",
            "CapitalismIsAPyramidScheme",
            "BreadPitt",
            "GrangerDanger"
        };

        public readonly List<string> _ownerEmail2 = new()
        {
            "@disnerd.com",
            "@hogwarts.com",
            "@galifrey.io",
            "@ocean.io",
            "@battlecrow.gov",
            "@grogusbakery.com",
            "@hedwigletters.io",
            "@kangaroopouch.com",
            "@nifflercoins.com",
            "@enterprise.gov",
            "@plantergeist.com",
            "@batfriends.gov",
            "@savethemanatees.gov",
            "@weepingangel.com",
            "@vamphunters.com"
        };


        /// <summary>
        /// Generates a number of random pets based on input.
        /// </summary>
        /// <param name="numberOfPets">The number of random pets to generate.</param>
        /// <returns>A list of random pets.</returns>
        public List<Pet> GenerateRandomPets(int numberOfPets)
        {

            var petList = new List<Pet>();

            for (var i = 0; i < numberOfPets; i++)
            {
                petList.Add(CreateRandomPet(i + 1));
            }

            return petList;
        }

        /// <summary>
        /// Uses random generators to build pet entries.
        /// </summary>
        /// <param name="id">ID to assign to the pet.</param>
        /// <returns>A randomly generated pet file.</returns>
        public Pet CreateRandomPet(int id)
        {
            var Email1 = _ownerEmail1[_rand.Next(0, 15)];
            var Email2 = _ownerEmail2[_rand.Next(0, 15)];

            return new Pet
            {
                Id = id,
                PetName = _petName[_rand.Next(0, 10)],
                Age = _rand.Next(1, 25),
                Weight = _rand.Next(1, 250),
                Species = _species[_rand.Next(0, 2)],
                Breed = _breed[_rand.Next(0, 9)],
                Fur = _fur[_rand.Next(0, 8)],
                OwnerName = _ownerName[_rand.Next(0, 10)],
                OwnerEmail = $"{Email1}{Email2}",
                OwnerPhone = GetRandomPhone()
            };
        }



        /// <summary>
        /// Generates a randomized pet SSN.
        /// </summary>
        /// <returns>A SSN string.</returns>
        public string GetRandomPhone()
        {
            var builder = new StringBuilder();
            builder.Append(_rand.Next(100, 999));
            builder.Append('-');
            builder.Append(_rand.Next(100, 999));
            builder.Append('-');
            builder.Append(_rand.Next(1000, 9999));

            return builder.ToString();
        }


    }
}
