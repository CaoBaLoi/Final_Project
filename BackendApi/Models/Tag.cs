using System.ComponentModel.DataAnnotations;

namespace Househole_shop.Models{
    public class Tag{
        [Required]
        public int tag_id {get;set;}
        [Required]
        public string? tag_name {get;set;}
    }
}