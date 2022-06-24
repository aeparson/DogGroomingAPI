namespace Grooming.DTOs
{
    /// <summary>
    /// Describes a data transfer object for a pet.
    /// </summary>
    public class PetDTO
    {
        public int Id { get; set; }

        public string PetName { get; set; }

        public int Age { get; set; }

        public int Weight { get; set; }

        public string Species { get; set; }

        public string Breed { get; set; }

        public string Fur { get; set; }

        public string OwnerName { get; set; }

        public string OwnerEmail { get; set; }

        public string OwnerPhone { get; set; }

        public int AppointmentCount { get; set; }
    }
}
