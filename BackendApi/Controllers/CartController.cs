using System.Security.Claims;
using Househole_shop.DTOs;
using Househole_shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                if (int.TryParse(userIdClaim.Value, out int user_id))
                {
                    // Now you can use userId safely
                    var items = await _cartService.GetCartItems(user_id);
                    return Ok(items);
                }
                else
                {
                    // Handle the case where userIdClaim.Value cannot be parsed to an integer
                    return BadRequest("Invalid user ID format");
                }
            }
            else
            {
                // Handle the case where ClaimTypes.NameIdentifier is not found
                return Unauthorized(); // Or some other appropriate response
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartDTO cartDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(); // Or handle the unauthorized case appropriately
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID format");
            }

            await _cartService.AddToCart(userId, cartDTO.product_id, cartDTO.quantity);
            return Ok();
        }

        [HttpDelete("{product_id}")]
        public async Task<IActionResult> RemoveFromCart(int product_id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(); // Or handle the unauthorized case appropriately
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID format");
            }

            await _cartService.RemoveFromCart(userId, product_id);
            return Ok();
        }
    }
}