using Api.Areas.HeThong.DTOs;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.HeThong.Controllers;

[Area("HeThong")]
public class DangNhapController : ControllerBase
{
	private readonly ILogger _logger;
	private readonly IQuanLyTaiKhoan _quanLyTaiKhoan;

	public DangNhapController(IQuanLyTaiKhoan quanLyTaiKhoan, ILogger<DangNhapController> logger)
	{
		_quanLyTaiKhoan = quanLyTaiKhoan;
		_logger = logger;
	}

	[HttpPost]
	public async Task<IActionResult> DangNhap(DangNhap.YeuCauDangNhap yeuCau)
	{
		bool flag = false;
		if (yeuCau.Password is null)
			if (yeuCau.Id is not null)
				flag = await _quanLyTaiKhoan.KiemTraId(yeuCau.Id.Value, HttpContext.RequestAborted);
			else if (yeuCau.UserName is not null)
				flag = await _quanLyTaiKhoan.KiemTraUsername(yeuCau.UserName, HttpContext.RequestAborted);
		if (flag)
		{
			string token = string.Empty;
			if (yeuCau.Id is not null)
				token = await _quanLyTaiKhoan.DangNhapAsync(yeuCau.Id.Value, yeuCau.Password!, HttpContext.RequestAborted);
			else if (yeuCau.UserName is not null)
				token = await _quanLyTaiKhoan.DangNhapAsync(yeuCau.UserName, yeuCau.Password!, HttpContext.RequestAborted);

			if (string.IsNullOrEmpty(token)) return Problem();
			_logger.LogInformation("Tạo token");
			return Ok(token);
		}
		return NotFound();
	}
}