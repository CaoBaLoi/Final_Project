using System.ComponentModel.DataAnnotations;

namespace Househole_shop.Models{
    public class Sale{
        [Required]
        public int sale_id {get;set;}
        [Required]
        public DateTime sale_begin_time {get;set;}
        [Required]
        public DateTime sale_end_time {get;set;}
    }
}