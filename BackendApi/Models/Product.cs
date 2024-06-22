using System.ComponentModel.DataAnnotations;

namespace Househole_shop.Models{
    public class Product{
        [Required]
        public int product_id {get;set;}
        [Required]
        public int category_id {get;set;}
        [Required]
        public string? product_name {get;set;}
        [Required]
        public float product_price {get;set;}
        [Required]
        public int product_quantity {get;set;}
        [Required]
        public string? product_description {get;set;}
    }
}