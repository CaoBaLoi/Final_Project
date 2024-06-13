using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Grocery.DTOs;
using System.Security.Claims;

namespace Grocery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            // Tạo đối tượng UserDataDTO từ các thông tin trong token
            var userData = new UserDataDTO
            {
                Id = id,
                Username = username,
                Roles = role
            };

            // Trả về thông tin người dùng
            return Ok(userData);
        }
    }
}
