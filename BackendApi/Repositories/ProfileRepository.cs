using Dapper;
using Househole_shop.DTOs;
using Househole_shop.Helpers;
using Househole_shop.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Househole_shop.Repositories{
    public interface IProfileRepository
	{
		Task<Profile?> GetProfileByUserIdAsync(string user_id);
		Task Create(string user_id, string fullname, string phone, string address);
		Task Update(string user_id, string fullname, string phone, string address);
	}
    public class ProfileRepository(DataContext context) : IProfileRepository
	{
		private readonly DataContext _context = context;

		public async Task<Profile?> GetProfileByUserIdAsync(string user_id)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
			SELECT * FROM Userprofile 
			WHERE user_id = @user_id";
            return await connection.QuerySingleOrDefaultAsync<Profile>(sql, new{user_id});
		}

		public async Task Create(string user_id, string fullname, string phone, string address)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
			INSERT INTO Userprofile (user_id, fullName, phone, address) 
			VALUES (@user_id, @fullName, @phone, @address)";
			await connection.ExecuteAsync(sql, new{ user_id, fullname, phone, address});
		}

		public async Task Update(string user_id, string fullname, string phone, string address)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
			UPDATE Userprofile 
			SET fullName = @fullName,
				phone    = @phone,
				address = @address, 
			WHERE user_id = @user_id";
			await connection.ExecuteAsync(sql, new{user_id, fullname, phone, address});
		}
    }
}