using System.ComponentModel.DataAnnotations;

namespace Grocery.DTOs{
    public class SaleItemDTO
    {
        [Required]
        public int product_id { get; set; }

        [Required]
        public float sale_percent{ get; set; }
    }
    public class SaleDTO{
        [Required]
        public List<SaleItemDTO>? sale_items {get;set;}
        [Required]
        public DateTime sale_begin_time {get;set;}
        [Required]
        public DateTime sale_end_time {get;set;}
    }
}