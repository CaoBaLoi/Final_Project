using Grocery.Models;
using Grocery.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grocery.Controllers{
    [ApiController]
	[Route("api/[controller]")]
	public class TagController(ITagService tagService) : ControllerBase
	{
		private readonly ITagService _tagService = tagService;

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var tags = await _tagService.GetAll();
			return Ok(tags);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var tag = await _tagService.GetById(id);
			return Ok(tag);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Tag tag)
		{
			await _tagService.Create(tag);
			return Ok(new { message = "Created successfully" });
		}

		[HttpPut]
		public async Task<IActionResult> Update(Tag tag)
		{
			await _tagService.Update(tag);
			return Ok(new { message = "Updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _tagService.Delete(id);
			return Ok(new { message = "Deleted successfully" });
		}

	}
}