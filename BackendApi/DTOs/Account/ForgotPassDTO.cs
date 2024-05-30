using System.ComponentModel.DataAnnotations;

namespace Grocery.DTOs{
    public class ForgotPassDTO{
        [Required]
        [EmailAddress]
        public required string Email {get;set;}
    }
}