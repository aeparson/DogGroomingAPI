using Grooming.Data.Context;
using Grooming.Data.SeedData;

namespace Grooming.Tests.Integration.Utilities
{
    public static class DatabaseSetupExtensions
    {
        public static void InitializeDatabaseForTests(this PetCtx context)
        {
            var petFactory = new PetFactory();
            var pets = petFactory.GenerateRandomPets(250);

            context.Pets.AddRange(pets);
            context.SaveChanges();
        }

        public static void ReinitializeDatabaseForTests(this PetCtx context)
        {
            context.Pets.RemoveRange(context.Pets);
            context.InitializeDatabaseForTests();
        }
    }
}
