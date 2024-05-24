using Grocery.Models;
using Grocery.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grocery.Controllers{
    [ApiController]
	[Route("api/[controller]")]
	public class SaleDetailController(ISaleDetailService saleDetailService) : ControllerBase
	{
		private readonly ISaleDetailService _saleDetailService = saleDetailService;

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var SaleDetails = await _saleDetailService.GetAll();
			return Ok(SaleDetails);
		}

		[HttpPost]
		public async Task<IActionResult> Create(SaleDetail saleDetail)
		{
			await _saleDetailService.Create(saleDetail);
			return Ok(new { message = "Created successfully" });
		}

		[HttpPut]
		public async Task<IActionResult> Update(SaleDetail saleDetail)
		{
			await _saleDetailService.Update(saleDetail);
			return Ok(new { message = "Updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _saleDetailService.Delete(id);
			return Ok(new { message = "Deleted successfully" });
		}

	}
}