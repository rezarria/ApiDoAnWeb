using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Api.Contexts;
using Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Protocol.Plugins;

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[ApiController]
[Route("[area]/[controller]")]
public partial class PhongHoc : ControllerBase
{
	private readonly AppDbContext _context;

	public PhongHoc(AppDbContext context)
	{
		_context = context;
	}

	/// <summary>
	/// Lấy thông tin về phòng học
	/// </summary>
	/// <param name="ids">Nếu có giá trị sẽ trả thông tin về phòng học có id</param>
	/// <returns>aaa</returns>
	/// <response code="200">Trả về danh sách phòng học, hoặc trả về một phòng học nếu id có giá trị</response>
	/// <response code="404">...</response>
	[ProducesResponseType(typeof(DTO.Get[]), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
	[HttpGet]
	public async Task<IActionResult> Get(string? ids)
	{
		Guid[]? array = ids?.Split(";").Select(x => new Guid(x)).ToArray() ?? null;
		if (array is not null && array.Length > 0)
			return Ok(GetMany(array));
		return await GetAll();
	}

	private async Task<IActionResult> GetAll()
	=> Ok(await
			_context.PhongHoc
			.Select(expressionGet)
			.AsNoTracking()
			.ToArrayAsync(HttpContext.RequestAborted)
		);

	private IEnumerable<dynamic> GetMany(Guid[] ids)
	=> _context.PhongHoc
		.Where(x => ids.Contains(x.Id))
		.Select(expressionGet)
		.AsNoTracking();

	/// <summary>
	/// Tạo phòng học
	/// </summary>
	/// <param name="phongHocDTO"></param>
	/// <returns>aaa</returns>
	/// <response code="201">Tạo thành công</response>
	/// <response code="500">...</response>
	[ProducesResponseType(typeof(DTO.Get[]), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
	[HttpPost]
	public async Task<IActionResult> Post(
	[Bind(
			nameof(phongHocDTO.Ten),
			nameof(phongHocDTO.ViTri)
		)]
		DTO.Post phongHocDTO
	)
	{
		IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
		try
		{
			ModelState.ClearValidationState(nameof(DTO.Post));
			var phongHoc = phongHocDTO.Convert();
			if (!TryValidateModel(phongHoc, nameof(Models.PhongHoc)))
				return BadRequest(ModelState);
			transaction.CreateSavepoint("begin");
			_context.Attach(phongHoc);
			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			transaction.Commit();
			return CreatedAtAction(nameof(Get), new { ids = new Guid[] { phongHoc.Id } }, phongHoc);
		}
		catch (System.Exception)
		{
			transaction.Rollback();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}


	/// <summary>
	/// Xoá về phòng học
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

		var phongHoc =
			from x in _context.PhongHoc
			where ids.Contains(x.Id)
			select new Models.PhongHoc()
			{
				Id = x.Id,
				RowVersion = x.RowVersion
			};


		transaction.CreateSavepoint("begin");

		if (phongHoc is null)
			return NotFound();
		try
		{
			await phongHoc.ForEachAsync(x => _context.Entry(x).State = EntityState.Deleted);
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
		await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
		await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
		try
		{
			var phongHoc = await _context.PhongHoc.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
			if (phongHoc is null)
				return NotFound();

			path.ApplyTo(phongHoc, ModelState);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _context.SaveChangesAsync(HttpContext.RequestAborted);
			transaction.Commit();

			return Ok(phongHoc);
		}
		catch (Exception)
		{
			transaction.Rollback();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}