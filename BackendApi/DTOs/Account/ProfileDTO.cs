using System.ComponentModel.DataAnnotations;

namespace Househole_shop.DTOs{
    public class ProfileDTO{
        public required string fullname { get; set; }
        public required string address { get; set; }
        public required string phone { get; set; }
    }
}