using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.HeThong.Controllers;

[Area("HeThong")]
[Route("[area]/[controller]")]
public class DangXuatController : ControllerBase
{
	private readonly IQuanLyTaiKhoan _quanLyTaiKhoan;

	public DangXuatController(IQuanLyTaiKhoan quanLyTaiKhoan)
	{
		_quanLyTaiKhoan = quanLyTaiKhoan;
	}
	[HttpPost]
	public IActionResult DangXuat(string token)
	{
		_quanLyTaiKhoan.DangXuat(token);
		return Ok();
	}
}