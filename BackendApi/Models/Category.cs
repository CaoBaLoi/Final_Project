using System.ComponentModel.DataAnnotations;

namespace Househole_shop.Models{
    public class Category{
        [Required]
        public int category_id {get;set;}
        [Required]
        public string? category_name {get;set;}
    }
}