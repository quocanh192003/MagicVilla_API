using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_Web.Models.dto
{
    public class VillaNumberDTO
    {
        required
        public int VillaNo { get; set; }   
        required
         public int villaID {get; set;}
        public string SpecialDetail { get; set; }  

    }
}
