using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_Web.Models.dto
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]   
        
        public int villaID {get; set;}
        public string SpecialDetail { get; set; }  
        public VillaDTO villa { get; set; }

    }
}
