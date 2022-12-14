using Api.Areas.Edu.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
public class DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung : ControllerBase
{
	private readonly AppDbContext _context;

	public DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung(AppDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	[Route("TheoKieuTaiKhoan")]
	public async Task<DTOs.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung.TruongThongTinNguoiDung[]> LayTheoKieuTaiKhoan(Guid id)
	=> await _context
			.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung
			.Where(x => x.IdKieuNguoiDung == id)
			.Select(DTOs.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung.TruongThongTinNguoiDung.Expression)
			.ToArrayAsync(HttpContext.RequestAborted);
}