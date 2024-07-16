using Dapper;
using Househole_shop.DTOs;
using Househole_shop.Helpers;
using Househole_shop.Models;
using Househole_shop.Services;

namespace Househole_shop.Repositories{
    public interface ISaleRepository
	{
		Task<IEnumerable<GetSaleDTO>> GetAll();
		Task Create(SaleDTO saleDTO);
		Task Update(Sale sale);
		Task Delete(int id);
		Task<GetSaleDTO> GetById(int id);
        Task<IEnumerable<GetSaleDTO>> GetByTime();
	}
    public class SaleRepository(DataContext context, SaleDetailService saleDetailService) : ISaleRepository
	{
		private readonly DataContext _context = context;
		private readonly SaleDetailService _saleDetailService = saleDetailService;

		public async Task<IEnumerable<GetSaleDTO>> GetAll()
        {
            using var connection = _context.CreateConnection();
            DateTime dateTimeNow = DateTime.Now;
            var sql = @"
                SELECT 
                    Sales.sale_id, 
                    Sales.sale_begin_time, 
                    Sales.sale_end_time,
                    Saledetail.product_id,
                    Saledetail.sale_percent,
                    Products.product_name, 
                    Products.product_price, 
                    Products.product_description, 
                    Productimage.image_url
                FROM Sales
                INNER JOIN Saledetail ON Sales.sale_id = Saledetail.sale_id
                INNER JOIN Products ON Saledetail.product_id = Products.product_id
                INNER JOIN Productimage ON Products.product_id = Productimage.product_id
                ORDER BY Sales.sale_id DESC
            ";

            var saleDictionary = new Dictionary<int, GetSaleDTO>();

            var sales = await connection.QueryAsync<GetSaleDTO, SaleItems, GetSaleDTO>(
                sql,
                (sale, saleItem) =>
                {
                    if (!saleDictionary.TryGetValue(sale.sale_id, out var currentSale))
                    {
                        currentSale = sale;
                        currentSale.saleItems = new List<SaleItems>();
                        saleDictionary.Add(currentSale.sale_id, currentSale);
                    }
					currentSale.saleItems ??= new List<SaleItems>();
                    currentSale.saleItems.Add(saleItem);
                    return currentSale;
                },
                new { dateTimeNow },
                splitOn: "product_id"
            );

            return sales.Distinct().ToList();
        }
		public async Task Create(SaleDTO saleDTO)
		{
			using var connection = _context.CreateConnection();
			var task = new List<Task>();
			var sql = @"
				INSERT INTO Sales (sale_begin_time, sale_end_time)
				VALUES (
                    CONVERT_TZ(@sale_begin_time, '+07:00', '+00:00'),
                    CONVERT_TZ(@sale_end_time, '+07:00', '+00:00')
                );
				SELECT LAST_INSERT_ID();
			";
			int saleId = await connection.QuerySingleAsync<int>(sql, new {
				saleDTO.sale_begin_time,
				saleDTO.sale_end_time
			});
			await Task.WhenAll(task);
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

        public async Task<GetSaleDTO> GetById(int id)
        {
            using var connection = _context.CreateConnection();
            DateTime dateTimeNow = DateTime.Now;
            var sql = @"
                SELECT 
                    Sales.sale_id, 
                    Sales.sale_begin_time, 
                    Sales.sale_end_time,
                    Saledetail.product_id,
                    Saledetail.sale_percent,
                    Products.product_name, 
                    Products.product_price, 
                    Products.product_description, 
                    Productimage.image_url
                FROM Sales
                INNER JOIN Saledetail ON Sales.sale_id = Saledetail.sale_id
                INNER JOIN Products ON Saledetail.product_id = Products.product_id
                INNER JOIN Productimage ON Products.product_id = Productimage.product_id
                WHERE Sales.sale_id = @id;
            ";

            var saleDictionary = new Dictionary<int, GetSaleDTO>();

            var sales = await connection.QueryAsync<GetSaleDTO, SaleItems, GetSaleDTO>(
                sql,
                (sale, saleItem) =>
                {
                    if (!saleDictionary.TryGetValue(sale.sale_id, out var currentSale))
                    {
                        currentSale = sale;
                        currentSale.saleItems = new List<SaleItems>();
                        saleDictionary.Add(currentSale.sale_id, currentSale);
                    }
					currentSale.saleItems ??= new List<SaleItems>();
                    currentSale.saleItems.Add(saleItem);
                    return currentSale;
                },
                new { id },
                splitOn: "product_id"
            );

            return sales.FirstOrDefault() ?? new GetSaleDTO
			{
				sale_id = 0, 
				sale_begin_time = DateTime.MinValue,
				sale_end_time = DateTime.MinValue,
				saleItems = new List<SaleItems>()
			};
        }

        public async Task<IEnumerable<GetSaleDTO>> GetByTime()
        {
            using var connection = _context.CreateConnection();
            DateTime dateTimeNow = DateTime.Now;
            var sql = @"
                SELECT 
                    Sales.sale_id, 
                    Sales.sale_begin_time, 
                    Sales.sale_end_time,
                    Saledetail.product_id,
                    Saledetail.sale_percent,
                    Products.product_name, 
                    Products.product_price, 
                    Products.product_description, 
                    Productimage.image_url
                FROM Sales
                INNER JOIN Saledetail ON Sales.sale_id = Saledetail.sale_id
                INNER JOIN Products ON Saledetail.product_id = Products.product_id
                INNER JOIN Productimage ON Products.product_id = Productimage.product_id
                WHERE @dateTimeNow > Sales.sale_begin_time 
                AND @dateTimeNow < Sales.sale_end_time;
            ";

            var saleDictionary = new Dictionary<int, GetSaleDTO>();

            var sales = await connection.QueryAsync<GetSaleDTO, SaleItems, GetSaleDTO>(
                sql,
                (sale, saleItem) =>
                {
                    if (!saleDictionary.TryGetValue(sale.sale_id, out var currentSale))
                    {
                        currentSale = sale;
                        currentSale.saleItems = new List<SaleItems>();
                        saleDictionary.Add(currentSale.sale_id, currentSale);
                    }
					currentSale.saleItems ??= new List<SaleItems>();
                    currentSale.saleItems.Add(saleItem);
                    return currentSale;
                },
                new { dateTimeNow },
                splitOn: "product_id"
            );

            return sales.Distinct().ToList();
        }
    }
}