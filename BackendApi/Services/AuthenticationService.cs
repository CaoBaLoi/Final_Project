using Dapper;
using Grocery.Helpers;
using Grocery.Models;
using Microsoft.AspNetCore.Identity;
namespace Grocery.Services{
public class AuthenticationService(DataContext context, UserManager<User> userManager, MailService mailService)
{
    private readonly DataContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly MailService _mailService = mailService;

    // public async Task<User?> AuthenticateUserAsync(string username, string password)
    // {
    //     using (var connection = _context.CreateConnection())
    //     {

    //         // Truy vấn kiểm tra xem người dùng có tồn tại và mật khẩu có khớp không
    //         var sql = "SELECT * FROM AspNetUsers WHERE UserName = @Username";
    //         var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserName = username });
    //         var passwordHash = new PasswordHasher<User>();
    //         if (user == null || user.PasswordHash == null || !VerifyPassword(password, user.PasswordHash))
    //         {
    //             return null; // Người dùng không tồn tại hoặc mật khẩu không khớp
    //         }
            
    //         return user;
    //     }
    // }

    // private bool VerifyPassword(string password, string passwordHash)
    // {
    //     // Triển khai hàm kiểm tra mật khẩu
    //     // Ví dụ: sử dụng ASP.NET Core Identity PasswordHasher
    //     var passwordHasher = new PasswordHasher<User>();
    //     var result = passwordHasher.VerifyHashedPassword(user, passwordHash, password);
    //     return result == PasswordVerificationResult.Success;
    // }
    public async Task<User?> AuthenticateUserAsync(string username, string password)
    {
        using (var connection = _context.CreateConnection())
        {
            // Query to get user information
            var userSql = "SELECT * FROM AspNetUsers WHERE UserName = @Username";
            var user = await connection.QueryFirstOrDefaultAsync<User>(userSql, new { Username = username });

            if (user == null)
            {
                return null; // User does not exist
            }

            // Assuming the password hash is stored in a different table named UserPasswords
            var passwordSql = "SELECT PasswordHash FROM AspNetUsers WHERE Id = @Id";
            var passwordHash = await connection.QueryFirstOrDefaultAsync<string>(passwordSql, new { user.Id });

            if (string.IsNullOrEmpty(passwordHash))
            {
                return null; // Password hash not found
            }

            // Verify the password using ASP.NET Core Identity PasswordHasher
            var passwordHasher = new PasswordHasher<User>();
            
            var result = passwordHasher.VerifyHashedPassword(user, passwordHash, password);
            if (result != PasswordVerificationResult.Success)
            {
                return null; // Password does not match
            }

            return user; // Authentication successful
        }
    }

    public async Task<bool> UserExist(string username){
        using (var connection = _context.CreateConnection()){
            var sql = "SELECT * FROM AspNetUsers WHERE UserName = @Username";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
            if (user == null)
            {
                return true; // Người dùng chưa tồn tại
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
            // Xử lý lỗi khác tùy thuộc vào yêu cầu của bạn
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


    // public async Task ResetPasswordAsync(string email, string otpCode, string newPassword)
    // {
    //     if (await VerifyOtpAsync(email, otpCode))
    //     {
    //         var user = await _userManager.FindByEmailAsync(email);
    //         if( user != null){
    //             var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
    //             await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
    //         }
    //         else{
    //             throw new Exception("User already exists");
    //         }
    //     }
    //     else
    //     {
    //         throw new Exception("Invalid OTP code");
    //     }
    // }
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
                        // Password reset successful
                        return;
                    }
                    else
                    {
                        // Password reset failed, log error details
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                        throw new Exception("Password reset failed");
                    }
                }
                catch (Exception ex)
                {
                    // Exception occurred during password reset, log error
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