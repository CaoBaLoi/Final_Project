using System.ComponentModel.DataAnnotations;

namespace Househole_shop.Models{
    public class Cart{
        [Required]
        public required int cart_id {get;set;}
        [Required]
        public required string uer_id {get;set;}
        [Required]
        public required int product_id {get;set;}
        [Required]
        public required int quantity {get;set;}
        public Product? Product {get;set;}
    }
}