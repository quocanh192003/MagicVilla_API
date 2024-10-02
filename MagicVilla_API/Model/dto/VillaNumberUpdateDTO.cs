using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Model.dto{
    public class VillaNumberUpdateDTO{
        required
        public int villaNo{get; set;}
        required
        public int villaID {get; set;}
        
        public string SpecialDetail{get; set;}
    }
}