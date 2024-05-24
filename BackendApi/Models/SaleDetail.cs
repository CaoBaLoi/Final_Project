using System.ComponentModel.DataAnnotations;

namespace Grocery.Models{
    public class SaleDetail{
        [Required]
        public int sale_deatil_id {get;set;}
        [Required]
        public int sale_id {get;set;}
        [Required]
        public int product_id {get;set;}
        [Required]
        public float sale_percent {get;set;}
    }
}