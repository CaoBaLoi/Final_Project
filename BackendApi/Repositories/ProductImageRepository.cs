using Dapper;
using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Repositories{
    public interface IProductImageRepository
	{
		Task<IEnumerable<ProductImage>> GetAll();
		//Task<ProductImage?> GetById(int id);
		Task Create(ProductImage productImage);
		Task Update(ProductImage productImage);
		Task <List<string>> GetUrlByProductId(int productId);
		Task<List<int>> GetIdByProductId(int productId);
		Task Delete(int id);
	}
    public class ProductImageRepository(DataContext context) : IProductImageRepository
	{
		private readonly DataContext _context = context;

		public async Task<IEnumerable<ProductImage>> GetAll()
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Productimage
			";
			return await connection.QueryAsync<ProductImage>(sql);
		}
		
		public async Task Create(ProductImage productImage)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				INSERT INTO Productimage (product_id, image_url)
				VALUES (@product_id, @image_url)
			";
			await connection.ExecuteAsync(sql, productImage);
		}
		public async Task Update(ProductImage productImage)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				UPDATE Productimage
				SET product_id = @product_id
                    image_url  = @image_url
				WHERE image_id = @image_id
			";
			await connection.ExecuteAsync(sql, productImage);
		}

        public async Task<List<string>> GetUrlByProductId(int productId)
        {
            using var connection = _context.CreateConnection();
			var sql = @"
				SELECT image_url FROM Productimage
				WHERE product_id = @productId
			";
            var productImage = await connection.QueryAsync<string>(sql, new { productId });
			var listImages = new List<string>();
			foreach (var listImage in productImage){
				listImages.Add(listImage);
			}
			return listImages;
        }
		
        public async Task Delete(int id)
        {
        	using var connection = _context.CreateConnection();
        	var sql = @"
        		DELETE FROM Productimage
        		WHERE image_id = @id
        	";
        	await connection.ExecuteAsync(sql, new { id });
        }

        public async Task<List<int>> GetIdByProductId(int productId)
        {
            using var connection = _context.CreateConnection();
			var sql = @"
				SELECT image_id FROM Productimage
				WHERE product_id = @productId
			";
			var imageIds = await connection.QueryAsync<int>(sql,new {productId});
			var listIds = new List<int>();
			foreach (var listId in imageIds){
				listIds.Add(listId);
			}
			return listIds;
        }
    }
}