using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Grooming.Data.Model
{
    public class Pet : BaseEntity
    {
        public string PetName { get; set; }

        public int Age { get; set; }

        public int Weight { get; set; }

        public string Species { get; set; }

        public string Breed { get; set; }

        public string Fur { get; set; }

        public string OwnerName { get; set; }

        public string OwnerEmail { get; set; }

        public string OwnerPhone { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
