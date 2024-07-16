namespace Househole_shop.Models{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public  string? PaymentMethod { get; set; }
        public  string? OrderDescription { get; set; }
        public  string? PaymentId { get; set; }
        public  string? TransactionId { get; set; }
        public  string? Token { get; set; }
        public  string? VnPayResponseCode { get; set; }
        public int order_id {get;set;}
        public string? product_id {get;set;}
        public string? order_detail_quantity {get;set;}
    }

}