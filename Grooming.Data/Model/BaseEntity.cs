using System.ComponentModel.DataAnnotations;

namespace Grooming.Data.Model
{
    /// <summary>
    /// This class represents a base for all other entities.
    /// </summary>
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

    }
}
