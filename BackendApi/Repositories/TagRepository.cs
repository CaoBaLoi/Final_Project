using Dapper;
using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Repositories{
    public interface ITagRepository
	{
		Task<IEnumerable<Tag>> GetAll();
		Task<Tag?> GetById(int id);
		Task Create(Tag tag);
		Task Update(Tag tag);
		Task Delete(int id);
		Task<Tag?> GetByName(string tagName);
		Task<int> GetOrCreateTagId(string tagName);

	}
    public class TagRepository(DataContext context) : ITagRepository
	{
		private readonly DataContext _context = context;

		public async Task<IEnumerable<Tag>> GetAll()
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Tags
			";
			return await connection.QueryAsync<Tag>(sql);
		}
		public async Task<Tag?> GetById(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				SELECT * FROM Tags
				WHERE tag_id = @id
			";
			return await connection.QuerySingleOrDefaultAsync<Tag>(sql, new { id });
		}
		public async Task Create(Tag tag)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				INSERT INTO Tags (tag_name)
				VALUES (@tag_name)
			";
			await connection.ExecuteAsync(sql, tag);
		}
		public async Task Update(Tag tag)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				UPDATE Tags
				SET tag_name = @tag_name
				WHERE tag_id = @tag_id
			";
			await connection.ExecuteAsync(sql, tag);
		}

		public async Task Delete(int id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
				DELETE FROM Tags
				WHERE tag_id = @id
			";
			await connection.ExecuteAsync(sql, new { id });
		}

        public async Task<Tag?> GetByName(string tagName)
        {
            using var connection = _context.CreateConnection();
			var sql = "SELECT * FROM Tags WHERE tag_name = @TagName";
			return await connection.QueryFirstOrDefaultAsync<Tag>(sql, new { TagName = tagName });
        }

        public async Task<int> GetOrCreateTagId(string tagName)
		{
			var tag = await GetByName(tagName);
			if (tag != null)
			{
				return tag.tag_id;
			}
			else
			{
				var newTag = new Tag { tag_name = tagName };
				await Create(newTag);

				while (true)
				{
					tag = await GetByName(tagName);
					if (tag != null)
					{
						return tag.tag_id;
					}
					await Task.Delay(100);
				}
			}
		}
    }
}