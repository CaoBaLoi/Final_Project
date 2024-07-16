
namespace Househole_shop.DTOs{
    public class SaleItemDTO
    {
 
        public int product_id { get; set; }


        public float sale_percent{ get; set; }
    }
    public class SaleDTO{

        public List<SaleItemDTO>? sale_items {get;set;}

        public DateTime sale_begin_time {get;set;}

        public DateTime sale_end_time {get;set;}
    }
}