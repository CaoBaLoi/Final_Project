using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
	[Route("api/[controller]")]
	public class ReceivedDateController(IReceivedDateService receivedDateService) : ControllerBase
	{
		private readonly IReceivedDateService _receivedDateService = receivedDateService;

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var receivedDate = await _receivedDateService.GetById(id);
			return Ok(receivedDate);
		}

		[HttpPost]
		public async Task<IActionResult> Create(int order_id, DateTime received_date)
		{
			await _receivedDateService.AddReceivedDate(order_id, received_date);
			return Ok(new { message = "Created successfully" });
		}

	}
}