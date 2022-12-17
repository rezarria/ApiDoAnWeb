#region

using System.Data;
using System.Linq.Expressions;
using Api.Areas.Edu.Contexts;
using Api.Areas.Edu.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

#endregion

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class NguoiDungController : ControllerBase
{
	private readonly AppDbContext _context;

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<NguoiDung, TOutputType>> expression,
		[FromQuery] Guid[]? ids,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		var query = _context.NguoiDung.AsQueryable();

		if (ids is not null && ids.Any())
			query = query.Where(x => ids.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	public NguoiDungController(AppDbContext context)
	{
		_context = context;
	}

	/// <summary>
	///     Lấy danh sách người dùng
	/// </summary>
	/// <param name="id">danh sách id cần lấy</param>
	/// <param name="take">số lượng lấy</param>
	/// <param name="skip">số lượng bỏ qua</param>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] Guid[]? id, [FromQuery] int take = -1, [FromQuery] int skip = 0)
		=> Ok(await Get(DTOs.NguoiDung.Get.Expression, id, take, skip));

	/// <summary>
	///     Tạo người dùng
	/// </summary>
	/// <param name="nguoiDungDto"></param>
	/// <returns></returns>
	/// <response code="201">Khi tạo người dùng</response>
	/// <response code="500">Khi có vấn đề</response>
	/// <response code="400">Khi sai thông tin</response>
	[ProducesResponseType(typeof(DTOs.NguoiDung.Get[]), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<IActionResult> Post([FromBody] DTOs.NguoiDung.Post nguoiDungDto)
	{
		NguoiDung nguoiDung = nguoiDungDto.Convert();
		ModelState.ClearValidationState(nameof(DTOs.NguoiDung.Post));
		if (!TryValidateModel(nguoiDung, nameof(NguoiDung)))
			return BadRequest(ModelState);
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			await transaction.CreateSavepointAsync("begin");
			_context.NguoiDung.Attach(nguoiDung);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();
			return CreatedAtAction(nameof(Get), new { ids = nguoiDung.Id.ToString() }, nguoiDung);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	///     Xoá người dùng theo id
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
			var danhSachNguoiDung = await (
				from x in _context.Lop
				where id.Contains(x.Id)
				select new NguoiDung
				{
					Id = x.Id,
					RowVersion = x.RowVersion
				}
			).ToListAsync(HttpContext.RequestAborted);
			danhSachNguoiDung.ForEach(lop => _context.Entry(lop).State = EntityState.Deleted);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			return Ok(danhSachNguoiDung.Select(x => x.Id));
		}

		return BadRequest();
	}

	/// <summary>
	///     Cập nhật người dùng theo id
	/// </summary>
	/// <param name="id">Guid</param>
	/// <param name="path">theo cấu trúc fast joson patch</param>
	/// <returns></returns>
	/// <response code="200">Cập nhật thành công và trả về kết quả</response>
	/// <response code="400">Khi validate thất bại</response>
	[ProducesResponseType(typeof(DTOs.NguoiDung.Get), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
	[HttpPatch]
	public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<NguoiDung> path)
	{
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
			if (nguoiDung is null)
				return NotFound();

			path.ApplyTo(nguoiDung, ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();

			return Ok(nguoiDung);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}