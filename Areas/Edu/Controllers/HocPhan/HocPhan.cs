using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Api.Areas.Api.Controllers;

[ApiController]
[Area("Api")]
[Route("/[area]/[controller]")]
public partial class HocPhan : ControllerBase
{
	private readonly Contexts.AppDbContext _context;

	public HocPhan(Contexts.AppDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public IActionResult Get(string? ids)
	{
		if (ids is null)
			return Ok(_context.HocPhan.AsNoTracking());
		var array = from x in ids?.Split(";") select new Guid(x);
		return Ok(
			from x in _context.HocPhan
			where array.Contains(x.Id)
			select x
		);
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] DTO.Post data)
	{
		ModelState.ClearValidationState(nameof(DTO.Post));
		Models.HocPhan hocPhan = data.Convert();
		TryValidateModel(hocPhan, nameof(Models.HocPhan));
		ModelState.Remove(ModelState
			.FirstOrDefault(x => x.Key.StartsWith($"{nameof(Models.HocPhan)}.{nameof(hocPhan.Mon)}")).Key);
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
			return CreatedAtAction(nameof(Get), new { ids = hocPhan.Id }, new[] { data });
		}
		catch (Exception)
		{
			transaction.Rollback();
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}