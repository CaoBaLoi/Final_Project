
namespace Househole_shop.DTOs{
    public class ProductDTO{

        public int product_id {get;set;}

        public required string category_name {get;set;}

        public required string[] tag_name {get;set;}
 
        public required List<IFormFile> image_file{get;set;}

        public string? product_name {get;set;}
 
        public int product_price {get;set;}
  
        public int product_quantity {get;set;}
  
        public string? product_description {get;set;}
    }
}