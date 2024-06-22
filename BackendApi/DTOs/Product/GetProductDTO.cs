using System.ComponentModel.DataAnnotations;

namespace Househole_shop.DTOs{
    public class GetProductDTO{
        [Required]
        public int product_id {get;set;}
        [Required]
        public required string category_name {get;set;}
        [Required]
        public required string[] tag_name {get;set;}
        [Required]
        public string? image_url{get;set;}
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