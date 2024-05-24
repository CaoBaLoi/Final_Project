using System.ComponentModel.DataAnnotations;

namespace Grocery.Models{
    public class Category{
        [Required]
        public int category_id {get;set;}
        [Required]
        public string? category_name {get;set;}
    }
}