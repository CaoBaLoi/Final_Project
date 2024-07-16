using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController (UserManager<User> userManager, AddressService AddressService) : ControllerBase{
        private readonly UserManager<User> _userManager = userManager;
        private readonly AddressService _AddressService = AddressService;
        
        [HttpGet]
        public async Task<IActionResult> GetAddress()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var Address = await _AddressService.GetAddressByUserIdAsync(user.Id);
                return Ok(Address);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDTO AddressDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            await _AddressService.Create(user.Id, AddressDTO.name, AddressDTO.phone, AddressDTO.address);
            return Ok(new { message = "Created successfully" });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressDTO AddressDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            await _AddressService.Update(user.Id, AddressDTO.name, AddressDTO.phone, AddressDTO.address);
            return Ok(new { message = "Updated successfully" });
        }
    }
}