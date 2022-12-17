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

[ApiController]
[Area("Api")]
[Route("/[area]/[controller]")]
public class HocPhan : ControllerBase
{
	private readonly AppDbContext _context;

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<Models.HocPhan, TOutputType>> expression,
		[FromQuery] Guid[]? ids,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		var query = _context.HocPhan.AsQueryable();

		if (ids is not null && ids.Any())
			query = query.Where(x => ids.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	public HocPhan(AppDbContext context)
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
		=> Ok(await Get(DTOs.HocPhan.Get.Expression, id, take, skip));

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] DTOs.HocPhan.Post data)
	{
		ModelState.ClearValidationState(nameof(DTOs.HocPhan.Post));
		Models.HocPhan hocPhan = data.Convert<DTOs.HocPhan.Post, Models.HocPhan>();
		TryValidateModel(hocPhan, nameof(Models.HocPhan));
		ModelState.Remove(ModelState
			.FirstOrDefault(x => x.Key.StartsWith($"{nameof(Models.HocPhan)}.{nameof(hocPhan.MonHoc)}")).Key);
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			await transaction.CreateSavepointAsync("begin");
			// byte[]? rowVersion = _context.Mon.Where(x => x.Id == hocPhan.Mon.Id).AsNoTracking().Select(x => x.RowVersion).FirstOrDefault();
			// if (rowVersion is null) throw new Exception();
			// hocPhan.Mon.RowVersion = rowVersion;
			_context.HocPhan.Attach(hocPhan);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();
			return CreatedAtAction(nameof(Get), new { ids = hocPhan.Id });
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
				from x in _context.HocPhan
				where id.Contains(x.Id)
				select new Models.HocPhan
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
    /// <response code="500">Khi validate thất bại</response>
    [ProducesResponseType(typeof(DTOs.LopHoc.Get), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	[HttpPatch]
	public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Models.HocPhan> path)
	{
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			var hocPhan = await _context.HocPhan.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
			if (hocPhan is null)
				return NotFound();

			path.ApplyTo(hocPhan, ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();

			return Ok(hocPhan);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}