using System.ComponentModel.DataAnnotations;

namespace Househole_shop.DTOs{
    public class ForgotPassDTO{
        [Required]
        [EmailAddress]
        public required string Email {get;set;}
    }
}