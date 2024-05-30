using Microsoft.AspNetCore.Identity;

namespace Grocery.Models{
    public class User : IdentityUser{
        public string? OTP {get;set;}
        public DateTime? OtpExpiration {get;set;}
    }
}