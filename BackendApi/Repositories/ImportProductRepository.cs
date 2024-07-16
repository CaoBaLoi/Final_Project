using Dapper;
using Househole_shop.DTOs;
using Househole_shop.Helpers;
using Househole_shop.Models;
using Househole_shop.Services;

namespace Househole_shop.Repositories{
    public interface IImportProductRepository{
        Task<IEnumerable<ImportProduct>> GetAll();
		Task Create(ImportProductDTO importProductDTO);
		Task Update(ImportProduct importProduct);
    }
    public class ImportProductRepository(DataContext context, IProductService productService) : IImportProductRepository
    {
        private readonly DataContext _context = context;
        private readonly IProductService _productService = productService;
        public async Task Create(ImportProductDTO importProductDTO)
        {
            using var connection = _context.CreateConnection();
			var sql = @"
				INSERT INTO Importproduct (product_id, import_date, import_price, import_quantity)
				VALUES (@product_id, @import_date, @import_price, @import_quantity)
			";
			await connection.ExecuteAsync(sql, new{
                importProductDTO.product_id,
                importProductDTO.import_date,
                importProductDTO.import_price,
                importProductDTO.import_quantity
            });
            await _productService.UpdateQuantity(importProductDTO.product_id, importProductDTO.import_quantity);
        }

        public async Task<IEnumerable<ImportProduct>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = @"
				SELECT *FROM Importproduct
                ORDER BY import_id
			";
            return await connection.QueryAsync<ImportProduct>(sql);
        }

        public async Task Update(ImportProduct importProduct)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
				UPDATE ImportProduct
				SET import_date      = @import_date,
					product_price    = @product_price,
					product_quantity = @product_quantity,
				WHERE import_id = @id
			";
            await connection.ExecuteAsync(sql, new
			{
				id = importProduct.import_id,
                importProduct.import_date,
                importProduct.import_price,
                importProduct.import_quantity
			});
        }
    }
}