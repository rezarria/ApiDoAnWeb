using System.Data;
using System.IO;
using System.Linq.Expressions;
using Api.Areas.Edu.Contexts;
using Api.Areas.Edu.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Areas.Edu.Controllers;

/// <summary>
///     RESTful API của môn
/// </summary>
[Area("Api")]
[ApiController]
[Route("/[area]/[controller]")]
public partial class MonHocController : ControllerBase
{
	/// <summary>
	/// </summary>
	private readonly AppDbContext _context;

	/// <summary>
	/// </summary>
	/// <param name="context"></param>
	public MonHocController(AppDbContext context)
	{
		_context = context;
	}

	/// <summary>
	/// lấy danh sách môn
	/// </summary>
	/// <param name="id"></param>
	/// <param name="take"></param>
	/// <param name="skip"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] Guid[]? id, [FromQuery] int take = -1, [FromQuery] int skip = 0)
		=> Ok(await Get(MonHoc.Get.Expression, id, take, skip));

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<Models.MonHoc, TOutputType>> expression,
		[FromQuery] Guid[]? id,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		var query = _context.Mon.AsQueryable();

		if (id is not null && id.Any())
			query = query.Where(x => id.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	/// <summary>
	///     Lấy thông tin ở mức tối thiểu
	/// </summary>
	/// <param name="id"></param>
	/// <param name="take"></param>
	/// <param name="skip"></param>
	/// <returns></returns>
	/// <response code="200"></response>
	[ProducesResponseType(typeof(object), 200)]
	[HttpGet]
	[Route("ToiThieu")]
	public async Task<IActionResult> GetToiThieu(
		[FromQuery] Guid[] id,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		=> Ok(await Get(MonHoc.Get.ExpressionToiThieu, id, take, skip));


	/// <summary>
	///     Tạo môn mới
	/// </summary>
	/// <param name="mon"></param>
	/// <returns></returns>
	/// <response code="200">Tạo môn mới thành công và trả về môn</response>
	[HttpPost]
	[ProducesResponseType(typeof(MonHoc.Post), 201)]
	public async Task<IActionResult> Post([FromBody] MonHoc.Post mon)
	{
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			Models.MonHoc monMoi = mon.Convert();

			_context.Attach(monMoi);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync(HttpContext.RequestAborted);

			return CreatedAtAction(nameof(Get), new { id = monMoi.Id }, monMoi);
		}
		catch (Exception)
		{
			transaction.Rollback();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	///     Cập nhật môn theo id
	/// </summary>
	/// <param name="id">Guid</param>
	/// <param name="path">theo cấu trúc fast joson patch</param>
	/// <returns></returns>
	/// <response code="200">Cập nhật môn mới thành công và trả về môn</response>
	[HttpPatch]
	[ProducesResponseType(typeof(MonHoc.Get), 200)]
	public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Models.MonHoc> path)
	{
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			var mon = await _context.Mon.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
			if (mon is null)
				return NotFound();

			path.ApplyTo(mon, ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			transaction.Commit();

			return Ok(mon);
		}
		catch (Exception)
		{
			transaction.Rollback();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	///     Xóa môn
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <response code="200">Xóa thành công môn</response>
	/// <response code="400">không tìm thấy môn</response>
	/// <response code="500">Lỗi</response>
	[HttpDelete]
	[ProducesResponseType(typeof(Guid[]), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete([FromBody] Guid[] id)
	{
		IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			List<Models.MonHoc> cacMonBiXoa = await _context.Mon
			.Where(x => id.Contains(x.Id))
			.Select(x => new Models.MonHoc()
			{
				Id = x.Id,
				RowVersion = x.RowVersion
			})
			.ToListAsync(HttpContext.RequestAborted);

			if (cacMonBiXoa is null) return NotFound();

			var danhSachXoaThanhCong = cacMonBiXoa.Select(x => x.Id).ToArray();


			cacMonBiXoa.ForEach(x => _context.Entry(x).State = EntityState.Deleted);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync(HttpContext.RequestAborted);
			return Ok(danhSachXoaThanhCong);
		}
		catch (Exception)
		{
			return new StatusCodeResult(500);
		}
	}
}