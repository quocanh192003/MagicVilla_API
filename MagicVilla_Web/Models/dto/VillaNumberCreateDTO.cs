using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_Web.Models.dto{
    public class VillaNumberCreateDTO{
        required
        public int villaNo{get; set;}
        required
        public int villaID {get; set;}
        public string SpecialDetail {get; set;}
    }
}