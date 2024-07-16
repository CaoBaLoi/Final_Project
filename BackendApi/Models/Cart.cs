
namespace Househole_shop.Models{
    public class Cart{
 
        public int cart_id {get;set;}
      
        public required string user_id {get;set;}
       
        public required int product_id {get;set;}
      
        public required int quantity {get;set;}
        public required Product Product {get;set;}
    }
}