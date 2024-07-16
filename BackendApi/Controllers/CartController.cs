using System.Security.Claims;
using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly UserManager<User> _userManager;

        public CartController(CartService cartService, UserManager<User> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized(); 
            }
            try
            {
                var items = await _cartService.GetCartItems(user.Id);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartDTO cartDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized();
            }
            try
            {
                await _cartService.AddToCart(user.Id, cartDTO.product_id, cartDTO.quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("{product_id}")]
        public async Task<IActionResult> RemoveFromCart( int product_id)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized();
            }
            try
            {
                await _cartService.RemoveFromCart(user.Id, product_id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}