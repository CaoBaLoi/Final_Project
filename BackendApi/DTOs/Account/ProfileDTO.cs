using System.ComponentModel.DataAnnotations;

namespace Househole_shop.DTOs{
    public class ProfileDTO{
        [Required]
        public required string user_id {get;set;}
        public string? fullname { get; set; }
        public string? address { get; set; }
        public string? phone { get; set; }
    }
}