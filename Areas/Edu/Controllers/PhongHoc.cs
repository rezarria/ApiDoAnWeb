using System.ComponentModel.DataAnnotations;
using System.Data;
using Api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Protocol.Plugins;

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[ApiController]
[Route("[area]/[controller]")]
public class PhongHoc : ControllerBase
{
	private AppDbContext _context;

	public PhongHoc(AppDbContext context)
	{
		_context = context;
	}

	/// <summary>
	/// Lấy thông tin về phòng học
	/// </summary>
	/// <param name="id">Nếu có giá trị sẽ trả thông tin về phòng học có id</param>
	/// <returns>Trả về danh sách phòng học, hoặc trả về một phòng học nếu id có giá trị</returns>
	/// <response code="200"></response>
	/// <response code="404"></response>
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[HttpGet]
	public async Task<IActionResult> Get(Guid? id)
	{
		return await (id is null ? GetAll() : GetSingle(id.Value));
	}

	private async Task<IActionResult> GetAll()
	=> Ok(
			await (
				from x in _context.PhongHoc
				select new
				{
					x.Id,
					x.Ten,
					x.ViTri
				}
			)
			.AsNoTracking()
			.ToArrayAsync(HttpContext.RequestAborted)
		);


	private async Task<IActionResult> GetSingle(Guid id)
	{
		var data = await (
				from x in _context.PhongHoc
				where x.Id == id
				select new
				{
					x.Id,
					x.Ten,
					x.ViTri
				}
			)
			.AsNoTracking()
			.FirstOrDefaultAsync();
		return data is null ? NotFound() : Ok(data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(
		[Bind(
			nameof(phongHoc.Id),
			nameof(phongHoc.Ten),
			nameof(phongHoc.ViTri)
		)]
		Models.PhongHoc phongHoc
		)
	{
		IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			transaction.CreateSavepoint("begin");
			_context.Attach(phongHoc);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			transaction.Commit();
			return Ok();
		}
		catch (System.Exception)
		{
			transaction.Rollback();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	[HttpDelete]
	public async Task<IActionResult> Delete(Guid id)
	{

		IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);

		Models.PhongHoc? phongHoc = await (
			from x in _context.PhongHoc
			where x.Id == id
			select new Models.PhongHoc()
			{
				Id = x.Id,
				RowVersion = x.RowVersion
			}
		)
		.FirstOrDefaultAsync(HttpContext.RequestAborted);


		transaction.CreateSavepoint("begin");

		if (phongHoc is null)
			return NotFound();
		try
		{
			_context.Entry(phongHoc).State = EntityState.Deleted;
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
}