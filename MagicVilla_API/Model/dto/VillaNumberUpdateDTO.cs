using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Model.dto{
    public class VillaNumberUpdateDTO{
		[Required]
		public int villaNo{get; set;}
		[Required]
		public int villaID {get; set;}
        
        public string SpecialDetail{get; set;}
    }
}