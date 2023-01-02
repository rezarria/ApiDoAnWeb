using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.HeThong.Controllers;

[Area("HeThong")]
[Route("[area]/[controller]")]
public class DangXuatController : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> DangXuat(string id)
	{
		return Ok();
	}
}