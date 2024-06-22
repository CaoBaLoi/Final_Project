using System.ComponentModel.DataAnnotations;

namespace Househole_shop.DTOs{
    public class CartDTO
    {
        [Required]
        public required int product_id {get;set;}
        [Required]
        public required int quantity {get;set;}
    }
}