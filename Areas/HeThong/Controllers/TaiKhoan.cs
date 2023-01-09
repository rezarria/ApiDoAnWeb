using Api.Areas.Edu.Models;
using Api.Areas.HeThong.DTOs;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.HeThong.Controllers;

[Area("HeThong")]
[Route("[area]/[controller]")]
public class TaiKhoanController : ControllerBase
{
	private readonly IQuanLyTaiKhoan _quanLyTaiKhoan;

	public TaiKhoanController(IQuanLyTaiKhoan quanLyTaiKhoan)
	{
		_quanLyTaiKhoan = quanLyTaiKhoan;
	}

	[HttpPost]
	public async Task<IActionResult> DangKy([FromBody] Taikhoan.YeuCauTaoMoi yeuCau)
	{
		TaiKhoan taikhoanMoi = yeuCau.ChuyenDoi();
		if (!ModelState.IsValid) return BadRequest(ModelState);
		try
		{
			await _quanLyTaiKhoan.DangKyAsync(taikhoanMoi, HttpContext.RequestAborted);
			return Ok();
		}
		catch (Exception e)
		{
			return Problem(e.Message);
		}
	}

	[HttpGet]
	public async Task<IActionResult> DanhSachTaiKhoan()
	{
		return Ok(await _quanLyTaiKhoan.DanhsachIdTaiKhoanAsync(cancellationToken: HttpContext.RequestAborted));
	}
}