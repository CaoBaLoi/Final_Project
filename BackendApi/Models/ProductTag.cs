using System.ComponentModel.DataAnnotations;

namespace Househole_shop.Models{
    public class ProductTag{
        [Required]
        public int producttag_id {get;set;}
        [Required]
        public int product_id {get;set;}
        [Required]
        public int tag_id {get;set;}
    }
}