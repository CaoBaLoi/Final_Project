using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
	[Route("api/[controller]")]
	public class ProductTagController(IProductTagService productTagService) : ControllerBase
	{
		private readonly IProductTagService _productTagService = productTagService;

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var productTags = await _productTagService.GetAll();
			return Ok(productTags);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var productTag = await _productTagService.GetById(id);
			return Ok(productTag);
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProductTag productTag)
		{
			await _productTagService.Create(productTag);
			return Ok(new { message = "Created successfully" });
		}

		[HttpPut]
		public async Task<IActionResult> Update(ProductTag productTag)
		{
			await _productTagService.Update(productTag);
			return Ok(new { message = "Updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _productTagService.Delete(id);
			return Ok(new { message = "Deleted successfully" });
		}

	}
}