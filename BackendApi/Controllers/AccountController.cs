using System.Security.Claims;
using Grocery.Models;
using Grocery.DTOs;
using Grocery.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;

namespace Grocery.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(SignInManager<User> signInManager, UserManager<User> userManager , AuthenticationService authenticationService) : ControllerBase
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly AuthenticationService _authenticationService = authenticationService;
        private readonly UserManager<User> _userManager = userManager;
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var user = await _authenticationService.AuthenticateUserAsync(login.Username, login.Password);
            if (user != null)
            {
                if (user.UserName == null) {throw new ArgumentNullException(nameof(user.UserName), "User name cannot be null.");}
                var result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, login.Remember, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    return Ok("Login success");
                }
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout");
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = register.Username, Email = register.Email };
                var check = await _authenticationService.UserExist(register.Username);
                if(check == true){
                    var result = await _userManager.CreateAsync(user, register.Password);
                    if (result.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user,"User");
                        if(roleResult.Succeeded){
                            // Đăng nhập người dùng sau khi đăng ký thành công
                            await _signInManager.SignInAsync(user, isPersistent: false);

                            // Đăng ký thành công
                            return Ok("Register success");
                        }
                        else{
                            return StatusCode(500,roleResult.Errors);
                        }                      
                    }
                    // Xử lý lỗi khi đăng ký không thành công
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else{
                    return BadRequest("Username already exists");
                }
            }
            // Trả về BadRequest nếu dữ liệu không hợp lệ
            return BadRequest(ModelState);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassDTO forgotPassDTO)
        {
            try
            {
                await _authenticationService.GenerateAndSendOtpAsync(forgotPassDTO.Email);
                return Ok("Otp send to your email");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, ResetPassDTO resetPassDTO)
        {
            if(ModelState.IsValid){
                try
                {
                    // Kiểm tra xem đối tượng resetPassDTO có null không
                    if (email == null)
                    {
                        return BadRequest("Email is null");
                    }

                    // Kiểm tra xem các thuộc tính của đối tượng resetPassDTO có null không
                    if (resetPassDTO.OTP == null || resetPassDTO.Password == null)
                    {
                        return BadRequest("OTP or Password is null");
                    }

                    // Gọi phương thức ResetPasswordAsync từ dịch vụ xác thực
                    await _authenticationService.ResetPasswordAsync(email, resetPassDTO.OTP, resetPassDTO.Password);
                    return Ok("Reset password success");
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi xảy ra trong quá trình xử lý, trả về BadRequest với thông báo lỗi
                    return BadRequest(new { message = ex.Message });
                }
            }
            return BadRequest(ModelState);
        }
    }
}