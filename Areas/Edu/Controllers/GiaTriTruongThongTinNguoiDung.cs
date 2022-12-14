using Api.Areas.Edu.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class GiaTriTruongThongTinNguoiDung : ControllerBase
{
	private readonly AppDbContext _context;

	public GiaTriTruongThongTinNguoiDung(AppDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	[Route("phutro")]
	public async Task<DTOs.GiaTriTruongThongTinNguoiDung.GetPhuTro[]> LayTheoKieuTaiKhoan(Guid id)
	=> await _context
			.GiaTriTruongThongTinNguoiDung
			.Where(x => x.IdNguoiDung == id)
			.Select(DTOs.GiaTriTruongThongTinNguoiDung.GetPhuTro.Expression)
			.ToArrayAsync(HttpContext.RequestAborted);
}