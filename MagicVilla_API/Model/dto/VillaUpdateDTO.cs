using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Model.dto
{
    public class villaUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]

        public int Sqft { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Amenity { get; set; }
    }
}
