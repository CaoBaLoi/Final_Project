using Dapper;
using Grocery.DTOs;
using Grocery.Helpers;
using Grocery.Models;
using Grocery.Services;

namespace Grocery.Repositories{
    public interface ISaleRepository
	{
		Task<IEnumerable<Sale>> GetAll();
		//Task<Sale?> GetById(int id);
		Task Create(SaleDTO saleDTO);
		Task Update(Sale sale);
		Task Delete(int id);
	}
    public class SaleRepository(DataContext context, SaleDetailService saleDetailService) : ISaleRepository
	{
		private readonly DataContext _context = context;
		private readonly SaleDetailService _saleDetailService = saleDetailService;

		public async Task<IEnumerable<Sale>> GetAll()
		{
			using var connection = _context.CreateConnection();
            DateTime dateTimeNow = DateTime.Now;
			var sql = @"
				SELECT * FROM Sales 
				INNER JOIN Saledetail
                WHERE @dateTimeNow > sale_begin_time 
                AND   @dateTimeNow < sale_end_time
			";
			return await connection.QueryAsync<Sale>(sql, new {dateTimeNow});
		}
		public async Task Create(SaleDTO saleDTO)
		{
			using var connection = _context.CreateConnection();
			var task = new List<Task>();
			var sql = @"
				INSERT INTO Sales (sale_begin_time, sale_end_time)
				VALUES (@sale_begin_time, @sale_end_time);
				SELECT LAST_INSERT_ID();
			";
			int saleId = await connection.QuerySingleAsync<int>(sql, new {
				saleDTO.sale_begin_time,
				saleDTO.sale_end_time
			});
			await Task.WhenAll(task);
			// Kiểm tra và thêm các sản phẩm vào bảng chi tiết bán hàng (SaleDetails) nếu có
			if (saleDTO.sale_items != null )
			{
				foreach (var saleItem in saleDTO.sale_items)
				{
					await _saleDetailService.Create(new SaleDetail {sale_id = saleId, product_id = saleItem.product_id, sale_percent = saleItem.sale_percent});
				}
			}
		}
		public async Task Update(Sale sale)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				UPDATE Sales
				SET sale_begin_time = @sale_begin_time,
                    sale_end_time   = @sale_end_time
				WHERE sale_id = @sale_id
			";
			await connection.ExecuteAsync(sql, sale);
		}

		public async Task Delete(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				DELETE FROM Sales
				WHERE sale_id = @id
			";
			await connection.ExecuteAsync(sql, new { id });
		}
    }
}