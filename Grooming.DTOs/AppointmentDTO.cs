namespace Grooming.DTOs
{
    /// <summary>
    /// Describes a data transfer object for an appointment.
    /// </summary>
    public class AppointmentDTO
    {
        public int Id { get; set; }

        public int PetId { get; set; }

        public string Shampoos { get; set; }

        public string Conditioners { get; set; }

        public string Groomer { get; set; }

        public string BathMethods { get; set; }

        public string BrushInfo { get; set; }

        public string BehaviorNotes { get; set; }

        public string AdditionalServices { get; set; }

        public string OwnerDebrief { get; set; }

        public string Notes { get; set; }

        public decimal TotalCost { get; set; }

        public string Date { get; set; }
    }
}

