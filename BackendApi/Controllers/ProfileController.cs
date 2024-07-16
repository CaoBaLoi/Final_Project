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
        
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var profile = await _profileService.GetProfileByUserIdAsync(user.Id);
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            await _profileService.Create(user.Id, profileDTO.fullname, profileDTO.phone, profileDTO.address);
            return Ok(new { message = "Created successfully" });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO profileDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            await _profileService.Update(user.Id, profileDTO.fullname, profileDTO.phone, profileDTO.address);
            return Ok(new { message = "Updated successfully" });
        }
    }
}