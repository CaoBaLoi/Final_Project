using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
	[Route("api/[controller]")]
	public class SaleController(ISaleService saleService) : ControllerBase
	{
		private readonly ISaleService _saleService = saleService;

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var sales = await _saleService.GetAll();
			return Ok(sales);
		}
		[HttpGet("by-time")]
		public async Task<IActionResult> GetByTime()
		{
			var sales = await _saleService.GetByTime();
			return Ok(sales);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var sales = await _saleService.GetById(id);
			return Ok(sales);
		}

		[HttpPost]
		public async Task<IActionResult> Create(SaleDTO saleDTO)
		{
			await _saleService.Create(saleDTO);
			return Ok(new { message = "Created successfully" });
		}

		[HttpPut]
		public async Task<IActionResult> Update(Sale sale)
		{
			await _saleService.Update(sale);
			return Ok(new { message = "Updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _saleService.Delete(id);
			return Ok(new { message = "Deleted successfully" });
		}

	}
}