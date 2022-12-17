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
using MonHoc = Api.Areas.Edu.Models.MonHoc;

#endregion

namespace Api.Areas.Edu.Controllers;

/// <summary>
/// </summary>
[Area("Api")]
[Route("/[area]/[controller]")]
[ApiController]
public class SoYeuLyLich : ControllerBase
{
	private readonly AppDbContext _context;

	public SoYeuLyLich(AppDbContext context)
	{
		_context = context;
	}

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<Models.SoYeuLyLich, TOutputType>> expression,
		[FromQuery] Guid[]? id,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		var query = _context.SoYeuLyLich.AsQueryable();

		if (id is not null && id.Any())
			query = query.Where(x => id.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	/// <summary>
	///     lấy danh sách sơ yếu lý lịch
	/// </summary>
	/// <param name="id"></param>
	/// <param name="take"></param>
	/// <param name="skip"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] Guid[]? id, [FromQuery] int take = -1, [FromQuery] int skip = 0)
		=> Ok(await Get(DTOs.SoYeuLyLich.Get.Expression, id, take, skip));

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
		=> Ok(await Get(DTOs.SoYeuLyLich.Get.ExpressionToiThieu, id, take, skip));


	/// <summary>
	///     Tạo môn mới
	/// </summary>
	/// <param name="soYeuLyLichDto"></param>
	/// <returns></returns>
	/// <response code="200">Tạo môn mới thành công và trả về môn</response>
	/// <response code="400">Validate thất bại</response>
	/// <response code="500"></response>
	[HttpPost]
	[ProducesResponseType(typeof(DTOs.SoYeuLyLich.Get[]), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Post([FromBody] DTOs.SoYeuLyLich.Post soYeuLyLichDto)
	{
		Models.SoYeuLyLich soYeuLyLich = soYeuLyLichDto.Convert<DTOs.SoYeuLyLich.Post, Models.SoYeuLyLich>();
		ModelState.ClearValidationState(nameof(DTOs.SoYeuLyLich.Post));
		if (!TryValidateModel(soYeuLyLich, nameof(Models.SoYeuLyLich))) return BadRequest(ModelState);

		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			_context.Attach(soYeuLyLich);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync(HttpContext.RequestAborted);

			return CreatedAtAction(nameof(Get), new { id = new[] { soYeuLyLich.Id } });
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	///     Cập nhật sơ yếu lý lịch theo id
	/// </summary>
	/// <param name="id">Guid</param>
	/// <param name="path">theo cấu trúc fast joson patch</param>
	/// <returns></returns>
	/// <response code="200">Cập nhật sơ yếu lý lịch thành công và trả về sơ yếu lý lịch</response>
	[HttpPatch]
	[ProducesResponseType(typeof(DTOs.SoYeuLyLich.Get), StatusCodes.Status200OK)]
	public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Models.SoYeuLyLich> path)
	{
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			var soYeuLyLich =
				await _context.SoYeuLyLich.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
			if (soYeuLyLich is null)
				return NotFound();

			path.ApplyTo(soYeuLyLich, ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();

			return Ok(soYeuLyLich);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	///     Xóa sơ yếu lý lịch
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <response code="200">Xóa thành công sơ yếu lý lịch</response>
	/// <response code="400">không tìm thấy sơ yếu lý lịch</response>
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
			List<MonHoc> cacSoYeuLyLichBiXoa = await _context.SoYeuLyLich
				.Where(x => id.Contains(x.Id))
				.Select(x => new MonHoc
				{
					Id = x.Id,
					RowVersion = x.RowVersion
				})
				.ToListAsync(HttpContext.RequestAborted);

			if (!cacSoYeuLyLichBiXoa.Any()) return NotFound();

			var danhSachXoaThanhCong = cacSoYeuLyLichBiXoa.Select(x => x.Id).ToArray();


			cacSoYeuLyLichBiXoa.ForEach(x => _context.Entry(x).State = EntityState.Deleted);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync(HttpContext.RequestAborted);
			return Ok(danhSachXoaThanhCong);
		}
		catch (Exception)
		{
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}