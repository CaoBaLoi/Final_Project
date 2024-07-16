
namespace Househole_shop.DTOs{
    public class GetProductDTO{
        
        public int product_id {get;set;}

        public required string category_name {get;set;}

        public required string[] tag_name {get;set;}
    
        public string? image_url{get;set;}
      
        public string? product_name {get;set;}
      
        public int product_price {get;set;}
      
        public int product_quantity {get;set;}
      
        public string? product_description {get;set;}
    }
}