using Househole_shop.Models;

namespace Househole_shop.DTOs{
    public class OrderDTO
    {
        public DateTime order_date { get; set; }
        public int order_total_price { get; set; }
        public string? order_status { get; set; }
        public string? order_address {get;set;}
        public string? payment_method {get;set;}
        public required List<OrderDetail> orderDetails { get; set; }
    }
}