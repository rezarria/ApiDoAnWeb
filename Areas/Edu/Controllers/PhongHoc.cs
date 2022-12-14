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

#endregion

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[ApiController]
[Route("[area]/[controller]")]
public class PhongHoc : ControllerBase
{
	private readonly AppDbContext _context;

	public PhongHoc(AppDbContext context)
	{
		_context = context;
	}

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<Models.PhongHoc, TOutputType>> expression,
		[FromQuery] Guid[]? id,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		IQueryable<Models.PhongHoc> query = _context.PhongHoc.AsQueryable();

		if (id is not null && id.Any())
			query = query.Where(x => id.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	/// <summary>
	///     lấy danh sách phòng học
	/// </summary>
	/// <param name="id"></param>
	/// <param name="take"></param>
	/// <param name="skip"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] Guid[]? id, [FromQuery] int take = -1, [FromQuery] int skip = 0)
	{
		return Ok(await Get(DTOs.PhongHoc.Get.Expression, id, take, skip));
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
	{
		return Ok(await Get(DTOs.PhongHoc.Get.ExpressionToiThieu, id, take, skip));
	}


	/// <summary>
	///     Tạo phòng học
	/// </summary>
	/// <param name="phongHocDto"></param>
	/// <returns>aaa</returns>
	/// <response code="201">Tạo thành công</response>
	/// <response code="400">Validate thất bại</response>
	/// <response code="500">...</response>
	[ProducesResponseType(typeof(DTOs.PhongHoc.Get[]), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	[HttpPost]
	public async Task<IActionResult> Post(
		DTOs.PhongHoc.Post phongHocDto
	)
	{
		ModelState.ClearValidationState(nameof(DTOs.PhongHoc.Post));
		Models.PhongHoc phongHoc = phongHocDto.Convert<DTOs.PhongHoc.Post, Models.PhongHoc>();
		if (!TryValidateModel(phongHoc, nameof(Models.PhongHoc)))
			return BadRequest(ModelState);
		IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			await transaction.CreateSavepointAsync("begin");
			_context.Attach(phongHoc);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();
			return CreatedAtAction(nameof(Get), new { ids = new[] { phongHoc.Id } }, phongHoc);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}


	/// <summary>
	///     Xoá về phòng học
	/// </summary>
	/// <param name="ids">Nếu có giá trị sẽ trả thông tin về phòng học có id</param>
	/// <response code="204">Khi xoá thành công</response>
	/// <response code="500">...</response>
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	[HttpDelete]
	public async Task<IActionResult> Delete(Guid[] ids)
	{
		IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);

		IQueryable<Models.PhongHoc> phongHoc =
			from x in _context.PhongHoc
			where ids.Contains(x.Id)
			select new Models.PhongHoc
				   {
					   Id = x.Id,
					   RowVersion = x.RowVersion
				   };


		await transaction.CreateSavepointAsync("begin");

		if (!phongHoc.Any())
			return NotFound();
		try
		{
			await phongHoc.ForEachAsync(x => _context.Entry(x).State = EntityState.Deleted);
			await _context.SaveChangesAsync();
			await transaction.CommitAsync();
			return NoContent();
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	///     Cập nhật phòng học theo id
	/// </summary>
	/// <param name="id">Guid</param>
	/// <param name="path">theo cấu trúc fast joson patch</param>
	/// <returns></returns>
	/// <response code="200">Cập nhật thành công và trả về kết quả</response>
	/// <response code="404">Khi không tìm thấy</response>
	[HttpPatch]
	[ProducesResponseType(typeof(PhongHoc), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(PhongHoc), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Models.PhongHoc> path)
	{
		await using IDbContextTransaction transaction =
			await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			Models.PhongHoc? phongHoc = await _context.PhongHoc.FirstOrDefaultAsync(predicate: x => x.Id == id, HttpContext.RequestAborted);
			if (phongHoc is null)
				return NotFound();

			path.ApplyTo(phongHoc, ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			await transaction.CommitAsync();

			return Ok(phongHoc);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}