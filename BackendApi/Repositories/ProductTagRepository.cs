using Dapper;
using Grocery.Helpers;
using Grocery.Models;

namespace Grocery.Repositories{
    public interface IProductTagRepository
	{
		Task<IEnumerable<ProductTag>> GetAll();
		Task<ProductTag?> GetById(int id);
		Task Create(ProductTag productTag);
		Task Update(ProductTag productTag);
		Task<List<int>> GetByProductId(int productId);
		//Task<bool> ProductTagExit(int productId, int tagId);
		Task Delete(int id);
	}
    public class ProductTagRepository(DataContext context) : IProductTagRepository
	{
		private readonly DataContext _context = context;

		public async Task<IEnumerable<ProductTag>> GetAll()
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Producttag
			";
			return await connection.QueryAsync<ProductTag>(sql);
		}
		public async Task<ProductTag?> GetById(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Producttag
				WHERE Producttag_id = @id
			";
			return await connection.QuerySingleOrDefaultAsync<ProductTag>(sql, new { id });
		}
		public async Task Create(ProductTag productTag)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				INSERT INTO Producttag (product_id, tag_id)
				VALUES (@product_id, @tag_id)
			";
			await connection.ExecuteAsync(sql, productTag);
		}
		public async Task Update(ProductTag productTag)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				UPDATE Producttag
				SET product_id = @product_id,
                    tag_id     = @tag_id
				WHERE producttag_id = @producttag_id
			";
			await connection.ExecuteAsync(sql, productTag);
		}

		public async Task Delete(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				DELETE FROM Producttag
				WHERE producttag_id = @id
			";
			await connection.ExecuteAsync(sql, new { id });
		}

        public async Task<List<int>> GetByProductId(int productId)
        {
            using var connection = _context.CreateConnection();
			var sql = @"
				SELECT producttag_id FROM Producttag
				WHERE product_id = @productId
			";
			var producttagId = await connection.QueryAsync<int>(sql,new {productId});
			var listIds = new List<int>();
			foreach (var listId in producttagId){
				listIds.Add(listId);
			}
			return listIds;
        }

        // public async Task<bool> ProductTagExit(int productId, int tagId)
        // {
        //     using var connection = _context.CreateConnection();
		// 	var sql = @"
		// 		SELECT *FROM Producttag
		// 		WHERE product_id = @productId AND tag_id = @tagId
		// 	";
		// 	int count = await connection.QueryFirstOrDefaultAsync<int>(sql, new { productId, tagId });
		// 	return count > 0;
        // }
    }
}