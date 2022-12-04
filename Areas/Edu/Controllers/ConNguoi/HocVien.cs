using System.Data;
using Api.Contexts;
using Api.PhuTro;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Api.Areas.Api.Controllers;

/// <summary>
/// </summary>
[Area("Api")]
[ApiController]
[Route("/[area]/hocvien")]
public class HocVien : ControllerBase
{
	private readonly AppDbContext _context;

	/// <summary>
	/// </summary>
	/// <param name="database"></param>
	public HocVien(AppDbContext database)
	{
		_context = database;
	}

	[HttpGet("chitiet")]
	public async Task<IActionResult> GetDetails(Guid id)
	{
		var data = await (
				from x in _context.HocVien
				where x.Id == id
				select new
				{
					x.Truong,
					x.PhuHuynh
				}
			).AsNoTracking().FirstOrDefaultAsync(HttpContext.RequestAborted);
		return (data is null) ? NotFound() : Ok(data);
	}

	/// <summary>
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var danhSach = await _context.HocVien.Include(x => x.SoYeuLyLich).AsNoTracking()
			.ToArrayAsync(HttpContext.RequestAborted);
		return Ok(danhSach);
	}

	/// <summary>
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("{id}")]
	public async Task<IActionResult> Get(Guid id)
	{
		var hocVien = await _context.HocVien.Include(x => x.SoYeuLyLich)
			.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
		return hocVien is null ? NotFound() : Ok(hocVien);
	}

	/// <summary>
	/// </summary>
	/// <param name="hocVien"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> Post([FromBody] Models.HocVien hocVien)
	{
		using var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable);
		try
		{
			transaction.CreateSavepoint("1");

			if (await hocVien.ChuanBiThemAsync(_context, HttpContext.RequestAborted))
				return Conflict(new
				{
					target = "TaiKhoan.TaiKhoanDangNhap",
					message = "Tài khoản đăng nhập đã tồn tại"
				});

			_context.HocVien.Attach(hocVien);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);

			transaction.Commit();

			return CreatedAtAction(nameof(Get), new { id = hocVien.Id }, hocVien);
		}
		catch (Exception)
		{
			transaction.RollbackToSavepoint("1");
			throw;
		}
	}

	/// <summary>
	/// </summary>
	/// <param name="ids"></param>
	/// <returns></returns>
	[HttpDelete]
	public async Task<IActionResult> Delete(Guid[] ids)
	{
		IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);

		var hocVien =
			from x in _context.HocVien
			where ids.Contains(x.Id)
			select new Models.HocVien()
			{
				Id = x.Id,
				RowVersion = x.RowVersion
			};


		transaction.CreateSavepoint("begin");

		if (hocVien is null)
			return NotFound();
		try
		{
			await hocVien.ForEachAsync(x => _context.Entry(x).State = EntityState.Deleted);
			_context.SaveChanges();
			transaction.Commit();
			return NoContent();
		}
		catch (System.Exception)
		{
			transaction.Rollback();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// </summary>
	/// <param name="id"></param>
	/// <param name="patch"></param>
	/// <returns></returns>
	[HttpPatch]
	public async Task<IActionResult> Update(Guid id, [FromBody] JsonPatchDocument<Models.HocVien> patch)
	{
		object?[] keys = new object?[] { id };
		Models.HocVien? hocVien = await _context.HocVien.FindAsync(keys, cancellationToken: HttpContext.RequestAborted);
		if (hocVien is null) return NotFound();
		await _context.Entry(hocVien).Reference(x => x.SoYeuLyLich).LoadAsync(HttpContext.RequestAborted);
		await _context.Entry(hocVien.SoYeuLyLich!).Reference(x => x.ChoOHienNay).LoadAsync(HttpContext.RequestAborted);
		using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("1", HttpContext.RequestAborted);
		patch.ApplyTo(hocVien, ModelState);
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
		try
		{
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			transaction.Commit();
			return Ok(hocVien);
		}
		catch (Exception)
		{
			transaction.RollbackToSavepoint("1");
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}