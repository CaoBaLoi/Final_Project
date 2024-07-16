using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
	[Route("api/[controller]")]
	public class ProductController(IProductService productService) : ControllerBase
	{
		private readonly IProductService _productService = productService;

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var products = await _productService.GetAll();
			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var product = await _productService.GetById(id);
			return Ok(product);
		}
		[HttpGet("quantity")]
		public async Task<IActionResult> GetQuantity(int product_id)
		{
			var quantity = await _productService.GetQuantity(product_id);
			return Ok(quantity);
		}
		[HttpPost]
		public async Task<IActionResult> Create(ProductDTO productDTO)
		{
			await _productService.Create(productDTO);
			return Ok(new { message = "Created successfully" });
		}

		[HttpPut]
		public async Task<IActionResult> Update(ProductDTO productDTO)
		{
			await _productService.Update(productDTO);
			return Ok(new { message = "Updated successfully" });
		}
		[HttpPut("quantity")]
		public async Task<IActionResult> Update(int product_id, int quantity)
		{
			await _productService.UpdateQuantity(product_id, quantity);
			return Ok(new { message = "Updated successfully" });
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _productService.Delete(id);
			return Ok(new { message = "Deleted successfully" });
		}
	}
}