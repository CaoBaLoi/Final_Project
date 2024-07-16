using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Househole_shop.DTOs;
using System.Security.Claims;

namespace Househole_shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("info")]
        public IActionResult GetUserInfo()
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var userData = new UserDataDTO
            {
                Id = id,
                Username = username,
                Roles = role
            };

            return Ok(userData);
        }
    }
}
