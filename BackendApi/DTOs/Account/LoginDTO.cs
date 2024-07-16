namespace Househole_shop.DTOs{
    public class LoginDTO{
        public required string Username {get;set;}
        public required string Password {get;set;}
        public required bool Remember {get;set;}
    }
}