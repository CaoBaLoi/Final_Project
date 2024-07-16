using Dapper;
using Househole_shop.DTOs;
using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Repositories{
    public interface IFeedBackRepository{
        Task<IEnumerable<FeedBack>> getAll();
        Task<IEnumerable<FeedBack>> getByUser(string user_id);
        Task addFeedBack(string user_id, string username, FeedBackDTO feedBackDTO);
    }
    public class FeedBackRepository(DataContext context):IFeedBackRepository{
        private readonly DataContext _context = context;

        public async Task addFeedBack(string user_id, string username, FeedBackDTO feedBackDTO)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Feedback (user_id, username, product_id, feedback, feedback_date, feedback_point)
                        VALUES (@user_id, @username, @product_id, @feedback, @feedback_date, @feedback_point);";
            await connection.ExecuteAsync(sql, new {user_id, feedBackDTO.product_id, username,  feedBackDTO.feedback, feedback_date = DateTime.Now, feedBackDTO.feedback_point });
        }

        public async Task<IEnumerable<FeedBack>> getAll()
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT *FROM Feedback;";
            return await connection.QueryAsync<FeedBack>(sql);
        }

        public async Task<IEnumerable<FeedBack>> getByUser(string user_id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT *FROM Feedback
                        WHERE user_id = @user_id;";
            return await connection.QueryAsync<FeedBack>(sql, new{user_id});
        }
    }
}