#region

using Api.Areas.Edu.Contexts;
using Api.Areas.Edu.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;
using TruongThongTinNguoiDung=Api.Areas.Edu.DTOs.TruongThongTinNguoiDung;

#endregion

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public class KieuNguoiDungController : ControllerBase
{
	private readonly AppDbContext _context;

	public KieuNguoiDungController(AppDbContext context)
	{
		_context = context;
	}

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<KieuNguoiDung, TOutputType>> expression,
		[FromQuery] Guid[]? ids,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		IQueryable<KieuNguoiDung> query = _context.KieuNguoiDung
												  .Include(x => x.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung)
												  .ThenInclude(x => x.TruongThongTinNguoiDung)
												  .AsQueryable();

		if (ids is not null && ids.Any())
			query = query.Where(x => ids.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	/// <summary>
	///     Lấy danh sách kiểu người dùng
	/// </summary>
	/// <param name="id"></param>
	/// <param name="take"></param>
	/// <param name="skip"></param>
	/// <returns></returns>
	/// <response code="200"></response>
	[ProducesResponseType(typeof(DTOs.KieuNguoiDung.Get[]), StatusCodes.Status200OK)]
	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] Guid[]? id, [FromQuery] int take = -1, [FromQuery] int skip = 0)
	{
		return Ok(await Get(DTOs.KieuNguoiDung.Get.Expression, id, take, skip));
	}

	/// <response code="200"></response>
	[HttpGet("toithieu")]
	public async Task<IActionResult> GetToiThieu(Guid[]? id, int take = -1, int skip = 0)
	{
		return Ok(await Get(expression: data => new
											    {
												    data.Id,
												    data.Ten
											    },
						    id,
						    take,
						    skip));
	}

	/// <summary>
	///     Tạo kiểu người dùng mới
	/// </summary>
	/// <param name="kieuNguoiDungDto"></param>
	/// <returns></returns>
	/// <response code="201">Khi tạo hành công</response>
	/// <response code="500">Khi có vấn đề</response>
	/// <response code="400">Khi sai thông tin</response>
	[ProducesResponseType(typeof(DTOs.LopHoc.Get[]), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<IActionResult> Post([FromBody] DTOs.KieuNguoiDung.Post kieuNguoiDungDto)
	{
		KieuNguoiDung kieuNguoiDung =
			kieuNguoiDungDto.Convert();
		ModelState.ClearValidationState(nameof(TruongThongTinNguoiDung.Post));
		if (!TryValidateModel(kieuNguoiDung, nameof(KieuNguoiDung)))
			return BadRequest(ModelState);
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			await transaction.CreateSavepointAsync("begin");
			_context.KieuNguoiDung.Attach(kieuNguoiDung);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();
			return CreatedAtAction(nameof(Get), new { ids = kieuNguoiDung.Id.ToString() }, kieuNguoiDung);
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
		if (id.Any())
		{
			List<KieuNguoiDung> danhSachKieuNguoiDungPhaiXoa = await (
																	     from x in _context.KieuNguoiDung
																	     where id.Contains(x.Id)
																	     select new KieuNguoiDung
																		        {
																			        Id = x.Id,
																			        RowVersion = x.RowVersion
																		        }
																     ).ToListAsync(HttpContext.RequestAborted);
			danhSachKieuNguoiDungPhaiXoa.ForEach(truongThongTin => _context.Entry(truongThongTin).State = EntityState.Deleted);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			return Ok(danhSachKieuNguoiDungPhaiXoa.Select(x => x.Id));
		}

		return BadRequest();
	}

	/// <summary>
	///     Cập nhật kiểu người dùng
	/// </summary>
	/// <param name="id">Guid</param>
	/// <param name="path">theo cấu trúc fast joson patch</param>
	/// <returns></returns>
	/// <response code="200">Cập nhật thành công và trả về kết quả</response>
	/// <response code="400">Khi validate thất bại</response>
	[ProducesResponseType(typeof(DTOs.LopHoc.Get), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
	[HttpPatch]
	public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<KieuNguoiDung> path)
	{
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			KieuNguoiDung? kieuNguoiDung =
				await _context.KieuNguoiDung.FirstOrDefaultAsync(predicate: x => x.Id == id, HttpContext.RequestAborted);

			if (kieuNguoiDung is null)
				return NotFound();

			path.ApplyTo(kieuNguoiDung, ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();

			return Ok(kieuNguoiDung);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	///     Cập nhật trường thông tin người dùng của kiểu người dùng theo id
	/// </summary>
	/// <param name="id">Id kiểu người dùng</param>
	/// <param name="idTruongThongTinNguoiDung">Mảng id trường thông tin được sử dụng</param>
	/// <returns></returns>
	/// <response code="200">Cập nhật thành công</response>
	/// <response code="404">Không tìm thấy kiểu tài khoản</response>
	/// <response code="500">Có vấn đề phát sinh</response>
	[HttpPatch]
	[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	[Route("TruongThongTinNguoiDung")]
	public async Task<IActionResult> CapNhatTruongThongTinNguoiDung(Guid id,
																	[FromBody] Guid[] idTruongThongTinNguoiDung)
	{
		if (!await _context.KieuNguoiDung.AnyAsync(predicate: x => x.Id == id, HttpContext.RequestAborted))
			return NotFound();

		IQueryable<Models.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung> query = _context.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung.Where(x => x.IdKieuNguoiDung == id);

		IQueryable<Models.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung> doiTuongXoa = (
																							   from x in query
																							   where !idTruongThongTinNguoiDung.Contains(x.IdTruongThongTinNguoiDung)
																							   select new Models.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung
																								      {
																									      Id = x.Id,
																									      RowVersion = x.RowVersion
																								      }
																						   ).AsNoTracking();

		List<Models.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung> doiTuongThem = (
																						  from x in idTruongThongTinNguoiDung
																						  where !query.Any(y => y.IdTruongThongTinNguoiDung == x)
																						  select new Models.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung
																							     {
																								     IdKieuNguoiDung = id,
																								     IdTruongThongTinNguoiDung = x
																							     }
																					  ).ToList();

		await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			await transaction.CreateSavepointAsync("begin", HttpContext.RequestAborted);
			await doiTuongXoa.ForEachAsync(action: x => _context.Entry(x).State = EntityState.Deleted, HttpContext.RequestAborted);
			_context.AttachRange(doiTuongThem);
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