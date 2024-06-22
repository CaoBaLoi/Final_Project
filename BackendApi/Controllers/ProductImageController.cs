using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
	[Route("api/[controller]")]
	public class ProductImageController(IProductImageService productImageService) : ControllerBase
	{
		private readonly IProductImageService _productImageService = productImageService;

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var productImage = await _productImageService.GetAll();
			return Ok(productImage);
		}

		// [HttpGet("{id}")]
		// public async Task<IActionResult> GetById(int id)
		// {
		// 	var productImage = await _productImageService.GetById(id);
		// 	return Ok(productImage);
		// }

		[HttpPost]
		public async Task<IActionResult> Create(ProductImage productImage)
		{
			await _productImageService.Create(productImage);
			return Ok(new { message = "Created successfully" });
		}

		[HttpPut]
		public async Task<IActionResult> Update(ProductImage productImage)
		{
			await _productImageService.Update(productImage);
			return Ok(new { message = "Updated successfully" });
		}

		// [HttpDelete("{id}")]
		// public async Task<IActionResult> Delete(int id)
		// {
		// 	await _productImageService.Delete(id);
		// 	return Ok(new { message = "Deleted successfully" });
		// }

	}
}