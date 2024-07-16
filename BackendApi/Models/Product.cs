
namespace Househole_shop.Models{
    public class Product{
       
        public int product_id {get;set;}
      
        public int category_id {get;set;}
      
        public string? product_name {get;set;}
     
        public int product_price {get;set;}
      
        public int product_quantity {get;set;}
    
        public string? product_description {get;set;}
        public List<string> Images { get; set; } = new List<string>();
    }
}