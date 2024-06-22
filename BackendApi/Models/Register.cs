using System.ComponentModel.DataAnnotations;

namespace Househole_shop.Models{
    public class Register{
        [Required]
        public required string Username {get;set;}
        [Required]
        public required string Email {get;set;}
        [Required]
        public required string Password {get;set;}
    }
}