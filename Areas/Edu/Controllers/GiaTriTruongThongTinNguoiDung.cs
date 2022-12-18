#region

using System.Linq.Expressions;
using Api.Areas.Edu.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

#endregion

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class GiaTriTruongThongTinNguoiDung : ControllerBase
{
	private readonly AppDbContext _context;

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<Models.GiaTriTruongThongTinNguoiDung, TOutputType>> expression,
		[FromQuery] Guid[]? ids,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		var query = _context.GiaTriTruongThongTinNguoiDung.AsQueryable();

		if (ids is not null && ids.Any())
			query = query.Where(x => ids.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	public GiaTriTruongThongTinNguoiDung(AppDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<DTOs.GiaTriTruongThongTinNguoiDung.Get[]> Get(Guid[] id, int take = -1, int skip = 0)
	=> await Get(DTOs.GiaTriTruongThongTinNguoiDung.Get.Expression, id, take, skip);

	[HttpGet]
	[Route("phutro")]
	public async Task<DTOs.GiaTriTruongThongTinNguoiDung.GetPhuTro[]> LayTheoKieuTaiKhoan(Guid id)
		=> await _context
			.GiaTriTruongThongTinNguoiDung
			.Where(x => x.IdNguoiDung == id)
			.Select(DTOs.GiaTriTruongThongTinNguoiDung.GetPhuTro.Expression)
			.ToArrayAsync(HttpContext.RequestAborted);

	[HttpPatch]
	[Route("CapNhatNho")]
	public async Task<IActionResult> CapNhatNho(DTOs.GiaTriTruongThongTinNguoiDung.Patch[] danhSach)
	{
		await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			var danhSachDoiTuong = await _context.GiaTriTruongThongTinNguoiDung
				.Where(x => true)
				.Select(x => new Models.GiaTriTruongThongTinNguoiDung()
				{
					Id = x.Id,
					GiaTri = danhSach.First(y => y.Id == x.Id).GiaTri,
					RowVersion = x.RowVersion
				})
				.AsNoTracking()
				.ToListAsync(HttpContext.RequestAborted);

			danhSachDoiTuong.ForEach(x =>
			{
				var entity = _context.Entry(x);
				entity.State = EntityState.Unchanged;
				entity.Property(x => x.GiaTri).EntityEntry.State = EntityState.Modified;
			});

			await transaction.CreateSavepointAsync("begin", HttpContext.RequestAborted);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync(HttpContext.RequestAborted);
			return Ok();
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return Problem();
		}
	}
}