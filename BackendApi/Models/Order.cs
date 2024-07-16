namespace Househole_shop.Models{
    public class Order
    {
        public int orders_id { get; set; }
        public required string user_id { get; set; }
        public DateTime order_date { get; set; }
        public int order_total_price { get; set; }
        public string? order_status { get; set; }
        public string? order_address {get;set;}
        public string? payment_method {get;set;}
        public required List<OrderDetail> orderDetails { get; set; }
    }
}