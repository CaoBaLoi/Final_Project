namespace Househole_shop.DTOs{
    public class SaleItems{
        public int product_id { get; set; }
        public string? product_name {get;set;}
        public int product_price {get;set;}
        public string? product_description {get;set;}
        public string? image_url {get;set;}
        public float sale_percent{ get; set; }
    }
    public class GetSaleDTO{    
        public int sale_id {get;set;}
        public DateTime sale_begin_time {get;set;}
        public DateTime sale_end_time {get;set;}
        public List<SaleItems>? saleItems {get;set;}
    }
}