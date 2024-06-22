using System.ComponentModel.DataAnnotations;

namespace Househole_shop.Models{
    public class ProductImage{
        [Required]
        public int image_id {get;set;}
        [Required]
        public int product_id {get;set;}
        [Required]
        public required string image_url {get;set;}
    }
}