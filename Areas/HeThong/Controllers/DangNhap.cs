using Api.Areas.HeThong.DTOs;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.HeThong.Controllers;

[Area("HeThong")]
[Route("[area]/[controller]")]
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
	public async Task<IActionResult> DangNhap([FromBody]DangNhap.YeuCauDangNhap yeuCau)
	{
		if (!await KiemTraTonTai(yeuCau))
			return NotFound();

		string token = await LayToken(yeuCau);
		if (string.IsNullOrEmpty(token)) return Problem();

		_logger.LogInformation("Tạo token");

		return Ok(token);
	}

	#region Private Method

	private async Task<string> LayToken(DangNhap.YeuCauDangNhap yeuCau)
	{
		string token = string.Empty;
		if (yeuCau.Id is not null)
			token = await _quanLyTaiKhoan.DangNhapAsync(yeuCau.Id.Value, yeuCau.Password!, HttpContext.RequestAborted);
		else if (!string.IsNullOrEmpty(yeuCau.UserName))
			token = await _quanLyTaiKhoan.DangNhapAsync(yeuCau.UserName, yeuCau.Password!, HttpContext.RequestAborted);
		return token;
	}
	private async Task<bool> KiemTraTonTai(DangNhap.YeuCauDangNhap yeuCau)
	{
		bool flag = false;
		if (!string.IsNullOrEmpty(yeuCau.Password))
			if (yeuCau.Id is not null)
				flag = await _quanLyTaiKhoan.KiemTraId(yeuCau.Id.Value, HttpContext.RequestAborted);
			else if (yeuCau.UserName is not null)
				flag = await _quanLyTaiKhoan.KiemTraUsername(yeuCau.UserName, HttpContext.RequestAborted);
		return flag;
	}

	#endregion
}