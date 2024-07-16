namespace Househole_shop.Models{
    public class FeedBack{
        public int feedback_id {get;set;}
        public string? user_id {get;set;}
        public string? username {get;set;}
        public int product_id {get;set;}
        public string? feedback {get;set;}
        public DateTime feedback_date {get;set;}
        public int feedback_point {get;set;}
    }
}