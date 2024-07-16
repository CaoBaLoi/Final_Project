using System.ComponentModel.DataAnnotations;

namespace Househole_shop.DTOs{
    public class RegisterDTO{
        public required string Username {get;set;}
        [EmailAddress]
        public required string Email {get;set;}
        public required string Password {get;set;}
    }
}