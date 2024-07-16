namespace Househole_shop.Models{
    public class OrderDetail
    {
        public int order_detail_id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int order_detail_quantity { get; set; }
        public int order_detail_price { get; set; }
        public string? image_url {get;set;}
        public string? product_name {get;set;}
    }
}