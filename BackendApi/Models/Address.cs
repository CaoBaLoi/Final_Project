namespace Househole_shop.Models{
    public class Address
{
    public int address_id { get; set; }
    public required string user_id{ get; set; }
    public string? name { get; set; }
    public string? address { get; set; }
    public string? phone { get; set; }
}
}