using System.ComponentModel.DataAnnotations;

namespace Househole_shop.Models{
    public class Login{
        [Required]
        public required string Username {get;set;}
        [Required]
        public required string Password {get;set;}
        [Required]
        public required bool Remember {get;set;}
    }
}