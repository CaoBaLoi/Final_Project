using Dapper;
using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Repositories{
    public interface ISaleDetailRepository
	{
		Task<IEnumerable<SaleDetail>> GetAll();
		Task Create(SaleDetail saleDetail);
		Task Update(SaleDetail saleDetail);
		Task Delete(int id);
	}
    public class SaleDetailRepository(DataContext context) : ISaleDetailRepository
	{
		private readonly DataContext _context = context;

		public async Task<IEnumerable<SaleDetail>> GetAll()
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Saledetail
			";
			return await connection.QueryAsync<SaleDetail>(sql);
		}
		public async Task Create(SaleDetail saleDetail)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				INSERT INTO Saledetail (sale_id, product_id, sale_percent)
				VALUES (@sale_id, @product_id, @sale_percent)
			";
			await connection.ExecuteAsync(sql, saleDetail);
		}
		public async Task Update(SaleDetail saleDetail)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				UPDATE Saledetail
				SET sale_id      = @sale_id,
                    product_id   = @product_id,
                    sale_percent = @sale_percent
				WHERE sale_detail_id = @sale_detail_id
			";
			await connection.ExecuteAsync(sql, saleDetail);
		}

		public async Task Delete(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				DELETE FROM Saledetail
				WHERE sale_detail_id = @id
			";
			await connection.ExecuteAsync(sql, new { id });
		}
    }
}