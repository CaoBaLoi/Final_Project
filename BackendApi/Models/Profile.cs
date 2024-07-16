
namespace Househole_shop.Models{
public class Profile
{
    public int profile_id { get; set; }
   
    public required string user_id{ get; set; }
    public string? fullname { get; set; }
    public string? address { get; set; }
    public string? phone { get; set; }
}
}