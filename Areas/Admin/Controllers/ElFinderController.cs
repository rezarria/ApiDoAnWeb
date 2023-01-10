using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
public class ElFinderController : ControllerBase
{
	private readonly QuanLyTepTin _quanLyTepTin;

	public ElFinderController(QuanLyTepTin quanLyTepTin)
	{
		_quanLyTepTin = quanLyTepTin;
	}

	[HttpGet("DongBoTaiKhoan")]
	public async Task<IActionResult> DongBoTaiKhoan()
	{
		await _quanLyTepTin.DongDoTaiKhoan(HttpContext.RequestAborted);
		return Ok();
	}
}