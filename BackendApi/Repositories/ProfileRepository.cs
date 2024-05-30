using Dapper;
using Grocery.DTOs;
using Grocery.Helpers;
using Grocery.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Grocery.Repositories{
    public interface IProfileRepository
	{
		Task<Profile?> GetProfileByUserIdAsync(string userId);
		Task Create(ProfileDTO profileDTO);
		Task Update(ProfileDTO profileDTO);
	}
    public class ProfileRepository(DataContext context) : IProfileRepository
	{
		private readonly DataContext _context = context;

		public async Task<Profile?> GetProfileByUserIdAsync(string userId)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
			SELECT * FROM Userprofile 
			WHERE uer_id = @user_id";
            return await connection.QuerySingleOrDefaultAsync<Profile>(sql, userId);
		}

		public async Task Create(ProfileDTO profileDTO)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
			INSERT INTO Userprofile (user_id, fullName, phone, address) 
			VALUES (@user_id, @fullName, @phone, @address)";
			await connection.ExecuteAsync(sql, profileDTO);
		}

		public async Task Update(ProfileDTO profileDTO)
		{
			using var connection = _context.CreateConnection();
			var sql = @"
			UPDATE Userprofile 
			SET fullName = @fullName,
				phone    = @phone,
				address = @address, 
			WHERE user_id = @user_id";
			await connection.ExecuteAsync(sql, profileDTO);
		}
    }
}