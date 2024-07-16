using Dapper;
using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Repositories{
    public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetAll();
		Task<Category?> GetById(int id);
		Task Create(Category category);
		Task Update(Category category);
		Task Delete(int id);
		Task<Category?> GetByName(string categoryName);
		Task<int> GetOrCreateCategoryId(string categoryName);
		//Task<Category?> CategoryExits(string categoryName);
	}
    public class CategoryRepository(DataContext context) : ICategoryRepository
	{
		private readonly DataContext _context = context;

		public async Task<IEnumerable<Category>> GetAll()
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Categories
				ORDER BY category_id DESC
			";
			return await connection.QueryAsync<Category>(sql);
		}
		public async Task<Category?> GetById(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Categories
				WHERE category_id = @id
			";
			return await connection.QuerySingleOrDefaultAsync<Category>(sql, new { id });
		}
		public async Task Create(Category category)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				INSERT INTO Categories (category_name)
				VALUES (@category_name)
			";
			await connection.ExecuteAsync(sql, category);
		}
		public async Task Update(Category category)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				UPDATE Categories
				SET category_name = @category_name
				WHERE category_id = @category_id
			";
			await connection.ExecuteAsync(sql, category);
		}

		public async Task Delete(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				DELETE FROM Categories
				WHERE category_id = @id
			";
			await connection.ExecuteAsync(sql, new { id });
		}

        public async Task<int> GetOrCreateCategoryId(string categoryName)
        {
			var category = await GetByName(categoryName);
			if (category != null)
			{
				return category.category_id;
			}
			else{
				var newCategory = new Category { category_name = categoryName };
				await Create(newCategory);

				while (true)
				{
					category = await GetByName(categoryName);
					if (category != null)
					{
						return category.category_id;
					}
					await Task.Delay(100);
				}
			}
		}

        public async Task<Category?> GetByName(string categoryName)
        {
            using var connection = _context.CreateConnection();
			var sql = "SELECT * FROM Categories WHERE category_name = @CategoryName";
			return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { CategoryName = categoryName });
        }
    }
}