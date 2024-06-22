using Microsoft.AspNetCore.Identity;

namespace Househole_shop.Models{
    public class User : IdentityUser{
        public string? OTP {get;set;}
        public DateTime? OtpExpiration {get;set;}
    }
}