#region

using Api.Areas.Edu.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Api.Areas.Edu.Controllers;

[Area("Api")]
[ApiController]
[Route("/[area]/[controller]")]
public class CaiDat : ControllerBase
{
	private readonly AppDbContext _context;

	public CaiDat(AppDbContext context)
	{
		_context = context;
	}

	[AcceptVerbs("GET")]
	public async Task<IActionResult> GetAll([FromQuery] int? skip = null, [FromQuery] int? take = null)
	{
		Models.CaiDat[] data;
		if (skip is null && take is null)
			data = await _context.CaiDat.ToArrayAsync(HttpContext.RequestAborted);
		else
		{
			skip ??= 0;
			take ??= 0;
			data = await _context.CaiDat.Skip(skip.Value).Take(take.Value).ToArrayAsync(HttpContext.RequestAborted);
		}

		return Ok(data);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(Guid id)
	{
		object[] objs = { id };
		Models.CaiDat? caiDat = await _context.CaiDat.FindAsync(objs, HttpContext.RequestAborted);
		if (caiDat is null)
			return NotFound();
		return Ok(caiDat);
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] CaiDatDto caiDat)
	{
		if (await _context.CaiDat.AnyAsync(predicate: x => x.Key == caiDat.Key, HttpContext.RequestAborted))
			return Conflict();
		_context.Add(caiDat as Models.CaiDat);
		await _context.SaveChangesAsync(HttpContext.RequestAborted);
		return Ok(caiDat);
	}

	[HttpPut]
	public async Task<IActionResult> Put([FromBody] CaiDatDto caiDat)
	{
		Models.CaiDat? caiDatMoi =
			await _context.CaiDat.Where(x => true).Take(1).FirstOrDefaultAsync(HttpContext.RequestAborted);
		if (caiDatMoi is null) return NotFound();
		caiDatMoi.Value = caiDat.Value;
		_context.Update(caiDatMoi);
		await _context.SaveChangesAsync(HttpContext.RequestAborted);
		return Ok(caiDatMoi);
	}

	[HttpDelete]
	public async Task<IActionResult> Delete(Guid id)
	{
		if (await IsExists(id)) return NotFound();
		Models.CaiDat caiDat = new()
							   {
								   Id = id
							   };
		_context.Entry(caiDat).State = EntityState.Deleted;
		await _context.SaveChangesAsync(HttpContext.RequestAborted);
		return Ok();
	}

	[NonAction]
	public async Task<bool> IsExists(string key)
	{
		return await _context.CaiDat.AnyAsync(predicate: x => x.Key == key, HttpContext.RequestAborted);
	}

	[NonAction]
	private async Task<bool> IsExists(Guid id)
	{
		return await _context.CaiDat.AnyAsync(predicate: x => x.Id == id, HttpContext.RequestAborted);
	}

	public class CaiDatDto : Models.CaiDat
	{
	}
}