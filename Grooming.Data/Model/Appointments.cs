
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Grooming.Data.Model
{
    public class Appointment : BaseEntity
    {
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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
