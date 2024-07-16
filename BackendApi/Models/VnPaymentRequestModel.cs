namespace Househole_shop.Models{
    public class VnPaymentRequestModel{
        public double Amount {get;set;}
        public DateTime CreatedDate { get; set; }
        public int order_id {get;set;}
        public string? product_id {get;set;}
        public string? order_detail_quantity {get;set;}
    }
}