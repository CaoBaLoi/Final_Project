using Grocery.DTOs;
using Grocery.Models;
using Grocery.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grocery.Controllers{
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

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _productService.Delete(id);
			return Ok(new { message = "Deleted successfully" });
		}
	}
}