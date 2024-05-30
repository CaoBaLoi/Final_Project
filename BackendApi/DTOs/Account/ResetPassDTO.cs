using System.ComponentModel.DataAnnotations;

namespace Grocery.DTOs{
    public class ResetPassDTO{
        [Required]
        public required string OTP {get;set;}
        [Required]
        public required string Password {get;set;}
    }
}