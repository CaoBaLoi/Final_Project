using System.ComponentModel.DataAnnotations;

namespace Grocery.Models{
public class Profile
{
    [Required]
    public int Id { get; set; }
    [Required]
    public required string user_id{ get; set; }
    public string? fullname { get; set; }
    public string? address { get; set; }
    public string? phone { get; set; }
}
}