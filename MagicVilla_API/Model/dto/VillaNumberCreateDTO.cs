using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Model.dto{
    public class VillaNumberCreateDTO{
        public int villaNo{get; set;}
        public string SpecialDetail {get; set;}
    }
}