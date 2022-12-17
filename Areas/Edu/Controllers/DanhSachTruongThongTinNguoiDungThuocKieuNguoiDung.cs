#region

using Api.Areas.Edu.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung : ControllerBase
{
	private readonly AppDbContext _context;

	public DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung(AppDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	[Route("TheoKieuNguoiDung")]
	public async Task<DTOs.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung.TruongThongTinNguoiDung[]>
		LayTheoKieuTaiKhoan(Guid id)
		=> await _context
			.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung
			.Where(x => x.IdKieuNguoiDung == id)
			.Select(DTOs.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung.TruongThongTinNguoiDung.Expression)
			.ToArrayAsync(HttpContext.RequestAborted);
}