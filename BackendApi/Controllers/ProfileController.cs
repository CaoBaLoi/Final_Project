using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController (UserManager<User> userManager, ProfileService profileService) : ControllerBase{
        private readonly UserManager<User> _userManager = userManager;
        private readonly ProfileService _profileService = profileService;
        private async Task<string?> GetUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = await GetUserIdAsync();
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                var profile = await _profileService.GetProfileByUserIdAsync(userId);
                return Ok(profile);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] ProfileDTO profileDTO)
        {
            var userId = await GetUserIdAsync();
            if (userId == null)
            {
                return Unauthorized();
            }

            profileDTO.user_id = userId;
            await _profileService.Create(profileDTO);
            return Ok(new { message = "Created successfully" });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO profileDTO)
        {
            var userId = await GetUserIdAsync();
            if (userId == null)
            {
                return Unauthorized();
            }

            profileDTO.user_id = userId;
            await _profileService.Update(profileDTO);
            return Ok(new { message = "Updated successfully" });
        }
    }
}