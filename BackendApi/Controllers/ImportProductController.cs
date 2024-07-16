using Househole_shop.DTOs;
using Househole_shop.Models;
using Househole_shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Househole_shop.Controllers{
    [ApiController]
	[Route("api/[controller]")]
	public class ImportProductController(IImportProductService importProductService) : ControllerBase
	{
        private readonly IImportProductService _importProductService = importProductService;
        [HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var imports = await _importProductService.GetAll();
			return Ok(imports);
		}


		[HttpPost]
		public async Task<IActionResult> Create(ImportProductDTO importProductDTO)
		{
			await _importProductService.Create(importProductDTO);
			return Ok(new { message = "Created successfully" });
		}

		[HttpPut]
		public async Task<IActionResult> Update(ImportProduct importProduct)
		{
			await _importProductService.Update(importProduct);
			return Ok(new { message = "Updated successfully" });
		}

    }
}