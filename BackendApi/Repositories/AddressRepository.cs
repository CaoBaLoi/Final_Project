using Dapper;
using Househole_shop.Helpers;
using Househole_shop.Models;

namespace Househole_shop.Repositories{
    public interface IAddressRepository{
        Task<IEnumerable<Address>> GetAddressByUserIdAsync(string user_id);
		Task Create(string user_id, string name, string phone, string address);
		Task Update(string user_id, string name, string phone, string address);
    }
    public class AddressRepository(DataContext context) : IAddressRepository
	{
		private readonly DataContext _context = context;

		public async Task<IEnumerable<Address>> GetAddressByUserIdAsync(string user_id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
			SELECT * FROM Address 
			WHERE user_id = @user_id";
            return await connection.QueryAsync<Address>(sql, new{user_id});
		}

		public async Task Create(string user_id, string name, string phone, string address)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
			INSERT INTO Address (user_id, name, phone, address) 
			VALUES (@user_id, @name, @phone, @address)";
			await connection.ExecuteAsync(sql, new{ user_id, name, phone, address});
		}

		public async Task Update(string user_id, string name, string phone, string address)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
			UPDATE Address 
			SET name = @name,
				phone    = @phone,
				address = @address, 
			WHERE user_id = @user_id";
			await connection.ExecuteAsync(sql, new{user_id, name, phone, address});
		}
    }
}