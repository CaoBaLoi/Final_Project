using CloudinaryDotNet.Actions;
using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedBackController(FeedBackService feedBackService, UserManager<User> userManager) : ControllerBase
    {
        private readonly FeedBackService _feedBackService = feedBackService;
        private readonly UserManager<User> _userManager = userManager;
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            try
            {
                var items = await _feedBackService.getAll();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("by-user")]
         public async Task<IActionResult> GetByUser(){
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized(); 
            }
            try
            {
                var items = await _feedBackService.getByUser(user.Id);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddFeedBack(FeedBackDTO feedBackDTO){
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); 
            }
            var username = await _userManager.GetUserNameAsync(user);
            if(username == null){
                return BadRequest("Username not found.");
            }
            try
            {
                await _feedBackService.addFeedBack(user.Id, username, feedBackDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}   