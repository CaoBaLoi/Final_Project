using Dapper;
using Househole_shop.Helpers;
using Househole_shop.Models;
using Microsoft.AspNetCore.Identity;
namespace Househole_shop.Services{
public class AuthenticationService(DataContext context, UserManager<User> userManager, MailService mailService)
{
    private readonly DataContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly MailService _mailService = mailService;

    public async Task<User?> AuthenticateUserAsync(string username, string password)
    {
        using (var connection = _context.CreateConnection())
        {

            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return user;
            }

            return null;
            }
    }

    public async Task<bool> UserExist(string username){
        using (var connection = _context.CreateConnection()){
            var sql = "SELECT * FROM AspNetUsers WHERE UserName = @Username";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
            if (user == null)
            {
                return true; 
            }
            return false;
        }
    }
    public async Task GenerateAndSendOtpAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var otpCode = new Random().Next(100000, 999999).ToString();
        var expirationTime = DateTime.Now.AddMinutes(5);
        try
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE AspNetUsers 
                SET OTP = @Otp, OTPExpiration = @Expiration 
                WHERE Id = @Id";
            await connection.ExecuteAsync(sql, new { OTP = otpCode, Expiration = expirationTime, Id = user.Id });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error executing SQL: " + ex.Message);
        }

        await _mailService.SendEmailAsync(email, "Your OTP Code", $"Your OTP code is {otpCode}");
    }

    public async Task<bool> VerifyOtpAsync(string email, string otpCode)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            using var connection = _context.CreateConnection();
            var sql = @"
                SELECT * FROM AspNetUsers 
                WHERE Id = @Id 
                AND OTP = @otpCode 
                AND OtpExpiration > @CurrentTime";
            
            var otp = await connection.QueryFirstOrDefaultAsync<User>(sql, new 
            { 
                user.Id, 
                otpCode, 
                CurrentTime = DateTime.Now 
            });
            return otp != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }

    public async Task ResetPasswordAsync(string email, string otpCode, string newPassword)
    {
        if (await VerifyOtpAsync(email, otpCode))
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                try
                {
                    var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                    if (result.Succeeded)
                    {
                        return;
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                        throw new Exception("Password reset failed");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred during password reset: {ex.Message}");
                    throw;
                }
            }
            else
            {
                throw new Exception("User not found");
            }
        }
        else
        {
            throw new Exception("Invalid OTP code");
        }
    }


}
}