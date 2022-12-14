#region

using Api.Areas.Edu.Contexts;
using Api.Areas.Edu.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;
using TruongThongTinNguoiDung=Api.Areas.Edu.Models.TruongThongTinNguoiDung;

#endregion

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class TruongThongTinNguoiDungController : ControllerBase
{
	private readonly AppDbContext _context;

	public TruongThongTinNguoiDungController(AppDbContext context)
	{
		_context = context;
	}

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<TruongThongTinNguoiDung, TOutputType>> expression,
		[FromQuery] Guid[]? ids,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		IQueryable<TruongThongTinNguoiDung> query = _context.TruongThongTinNguoiDung.AsQueryable();

		if (ids is not null && ids.Any())
			query = query.Where(x => ids.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	/// <summary>
	///     Lấy danh sách trường thông tin người dùng
	/// </summary>
	/// <param name="id"></param>
	/// <param name="take"></param>
	/// <param name="skip"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] Guid[]? id, [FromQuery] int take = -1, [FromQuery] int skip = 0)
	{
		return Ok(await Get(DTOs.TruongThongTinNguoiDung.Get.Expression, id, take, skip));
	}

	/// <summary>
	///     Tạo trường thông tin người dùng
	/// </summary>
	/// <param name="truongThongTinDto"></param>
	/// <returns></returns>
	/// <response code="201">Khi tạo hành công</response>
	/// <response code="500">Khi có vấn đề</response>
	/// <response code="400">Khi sai thông tin</response>
	[ProducesResponseType(typeof(DTOs.LopHoc.Get[]), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<IActionResult> Post([FromBody] DTOs.TruongThongTinNguoiDung.Post truongThongTinDto)
	{
		TruongThongTinNguoiDung truongThongTin =
			truongThongTinDto.Convert<DTOs.TruongThongTinNguoiDung.Post, TruongThongTinNguoiDung>();
		ModelState.ClearValidationState(nameof(DTOs.TruongThongTinNguoiDung.Post));
		if (!TryValidateModel(truongThongTin, nameof(TruongThongTinNguoiDung)))
			return BadRequest(ModelState);
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			await transaction.CreateSavepointAsync("begin");
			_context.TruongThongTinNguoiDung.Attach(truongThongTin);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();
			return CreatedAtAction(nameof(Get), new { ids = truongThongTin.Id.ToString() }, truongThongTin);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	///     Xoá trường thông tin người dùng học theo id
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
		if (!id.Any()) return BadRequest();
		List<TruongThongTinNguoiDung> danhSachTruong = await (
															     from x in _context.TruongThongTinNguoiDung
															     where id.Contains(x.Id)
															     select new TruongThongTinNguoiDung
																        {
																	        Id = x.Id,
																	        RowVersion = x.RowVersion
																        }
														     ).ToListAsync(HttpContext.RequestAborted);
		danhSachTruong.ForEach(truongThongTin => _context.Entry(truongThongTin).State = EntityState.Deleted);
		await _context.SaveChangesAsync(HttpContext.RequestAborted);
		return Ok(danhSachTruong.Select(x => x.Id));
	}

	/// <summary>
	///     Cập nhật trường thông tin người dùng học theo id
	/// </summary>
	/// <param name="id">Guid</param>
	/// <param name="patch">theo cấu trúc fast joson patch</param>
	/// <returns></returns>
	/// <response code="200">Cập nhật thành công và trả về kết quả</response>
	/// <response code="400">Khi validate thất bại</response>
	[ProducesResponseType(typeof(DTOs.LopHoc.Get), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
	[HttpPatch]
	public async Task<IActionResult> Patch([FromQuery] Guid id,
										   [FromBody] JsonPatchDocument<TruongThongTinNguoiDung> patch)
	{
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			TruongThongTinNguoiDung? truongThongTinNguoiDung =
				await _context.TruongThongTinNguoiDung.FirstOrDefaultAsync(predicate: x => x.Id == id, HttpContext.RequestAborted);
			if (truongThongTinNguoiDung is null)
				return NotFound();

			patch.ApplyTo(truongThongTinNguoiDung, ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync(HttpContext.RequestAborted);

			return Ok(truongThongTinNguoiDung);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}