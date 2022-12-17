#region

using System.Data;
using System.Linq.Expressions;
using Api.Areas.Edu.Contexts;
using Api.Areas.Edu.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

#endregion

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class LopHoc : ControllerBase
{
	private readonly AppDbContext _context;

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<Models.LopHoc, TOutputType>> expression,
		[FromQuery] Guid[]? ids,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		var query = _context.Lop.AsQueryable();

		if (ids is not null && ids.Any())
			query = query.Where(x => ids.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	public LopHoc(AppDbContext context)
	{
		_context = context;
	}

	/// <summary>
	///     Lấy danh sách lớp học
	/// </summary>
	/// <param name="id"></param>
	/// <param name="take"></param>
	/// <param name="skip"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] Guid[]? id, [FromQuery] int take = -1, [FromQuery] int skip = 0)
		=> Ok(await Get(DTOs.LopHoc.Get.Expression, id, take, skip));

	/// <summary>
	///     Tạo lớp học
	/// </summary>
	/// <param name="lopHoc"></param>
	/// <returns></returns>
	/// <response code="201">Khi tạo lớp thành công</response>
	/// <response code="500">Khi có vấn đề</response>
	/// <response code="400">Khi sai thông tin</response>
	[ProducesResponseType(typeof(DTOs.LopHoc.Get[]), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<IActionResult> Post([FromBody] DTOs.LopHoc.Post lopHoc)
	{
		Models.LopHoc lop = lopHoc.Convert<DTOs.LopHoc.Post, Models.LopHoc>();
		ModelState.ClearValidationState(nameof(DTOs.LopHoc.Post));
		if (!TryValidateModel(lop, nameof(Models.LopHoc)))
			return BadRequest(ModelState);
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			await transaction.CreateSavepointAsync("begin");
			_context.Lop.Attach(lop);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();
			return CreatedAtAction(nameof(Get), new { ids = lop.Id.ToString() }, lop);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	///     Xoá lớp học theo id
	/// </summary>
	/// <param name="id">các id đối tượng muốn xoá</param>
	/// <returns></returns>
	/// <response code="200">trả về danh sách id thành công</response>
	/// <response code="404">Khi không tìm thấy</response>
	/// <response code="400">Khi không có id</response>
	[ProducesResponseType(typeof(Guid[]), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
	[HttpDelete]
	public async Task<IActionResult> Delete([FromBody] Guid[] id)
	{
		if (id.Any())
		{
			var danhSachLop = await (
				from x in _context.Lop
				where id.Contains(x.Id)
				select new Models.LopHoc
				{
					Id = x.Id,
					RowVersion = x.RowVersion
				}
			).ToListAsync(HttpContext.RequestAborted);
			danhSachLop.ForEach(lop => _context.Entry(lop).State = EntityState.Deleted);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			return Ok(danhSachLop.Select(x => x.Id));
		}

		return BadRequest();
	}

	/// <summary>
	///     Cập nhật lớp học theo id
	/// </summary>
	/// <param name="id">Guid</param>
	/// <param name="path">theo cấu trúc fast joson patch</param>
	/// <returns></returns>
	/// <response code="200">Cập nhật thành công và trả về kết quả</response>
	/// <response code="400">Khi validate thất bại</response>
	[ProducesResponseType(typeof(DTOs.LopHoc.Get), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
	[HttpPatch]
	public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Models.LopHoc> path)
	{
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			var lopHoc = await _context.Lop.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
			if (lopHoc is null)
				return NotFound();

			path.ApplyTo(lopHoc, ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();

			return Ok(lopHoc);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}