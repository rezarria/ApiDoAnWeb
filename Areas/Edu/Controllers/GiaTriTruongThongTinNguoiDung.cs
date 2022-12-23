#region

using System;
using System.Linq.Expressions;
using Api.Areas.Edu.Contexts;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
	public async Task<IActionResult> CapNhatNho([FromQuery] Guid id, [FromBody] DTOs.GiaTriTruongThongTinNguoiDung.Patch[] danhSach)
	{
		if (!danhSach.Any()) return BadRequest();

		await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			XuLyCoId(id, danhSach);

			await XuLyKoCoId(id, danhSach);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync(HttpContext.RequestAborted);

			return Ok();
		}
		catch (Exception e)
		{
			await transaction.RollbackAsync();
			return Problem(e.Message);
		}
	}

	private async Task XuLyKoCoId(Guid idNguoiDung, DTOs.GiaTriTruongThongTinNguoiDung.Patch[] danhSach)
	{
		var koCoId = danhSach
			.Where(x => !x.Id.HasValue)
			.Select(DTOs.GiaTriTruongThongTinNguoiDung.Patch.Convert)
			.ToList();

		if (koCoId.Any())
		{
			if (_context.GiaTriTruongThongTinNguoiDung.Any(x => x.IdNguoiDung == idNguoiDung && koCoId.Select(y => y.IdTruongThongTinNguoiDung).Contains(x.IdTruongThongTinNguoiDung)))
				throw new Exception("Một số id đã tồn tại");

			koCoId.ForEach(x =>
			{
				x.IdNguoiDung = idNguoiDung;
			});

			await _context.AddRangeAsync(koCoId, HttpContext.RequestAborted);
		}
	}

	private void XuLyCoId(Guid idNguoiDung, DTOs.GiaTriTruongThongTinNguoiDung.Patch[] danhSach)
	{
		var coId = danhSach
			.Where(x => x.Id.HasValue)
			.Select(DTOs.GiaTriTruongThongTinNguoiDung.Patch.Convert)
			.ToList();

		if (coId.Any())
		{
			var danhSachKey = coId.Select(x => x.Id);

			var a = new Dictionary<Guid, byte[]>();

			var danhSachRowVersion = _context.GiaTriTruongThongTinNguoiDung
				.Where(x => danhSachKey.Contains(x.Id) && x.IdNguoiDung == idNguoiDung)
				.Select(x => new KeyValuePair<Guid, byte[]?>(x.Id, x.RowVersion))
				.ToList();

			if (danhSachRowVersion.Count() != danhSachKey.Count())
				throw new Exception("Không thuộc về người dùng này");

			danhSachRowVersion.ForEach(x =>
			{
				var giaTri = coId.First(y => y.Id == x.Key);
				giaTri.RowVersion = x.Value;

				var entity = _context.Entry(giaTri);

				entity.Properties.AsQueryable().ForEachAsync(x => x.IsModified = false);

				entity.Property(x => x.GiaTri).IsModified = true;
			});
		}
	}
}