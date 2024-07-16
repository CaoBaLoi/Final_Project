using Dapper;
using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Repositories{
    public interface IReceivedDateRepository{
        Task AddReceivedDate(int order_id, DateTime received_date);
        Task<ReceivedDate?> GetById(int id);
    }
    public class ReceivedDateRepository(DataContext context) : IReceivedDateRepository
    {
        private readonly DataContext _context = context;
        public async Task AddReceivedDate(int order_id, DateTime received_date)
        {
            using var connection = _context.CreateConnection();
			var sql = @"
				INSERT INTO Receiveddate (order_id, received_date)
				VALUES (@order_id, @received_date)
			";
			await connection.ExecuteAsync(sql,new{order_id, received_date});
        }

        public async Task<ReceivedDate?> GetById(int id)
        {
            using var connection = _context.CreateConnection();
			var sql = @"
				SELECT *FROM Receiveddate
                WHERE order_id = @id
			";
			var receivedDate = await connection.QueryFirstOrDefaultAsync<ReceivedDate>(sql, new { id });

            return receivedDate;
        }
    }
}