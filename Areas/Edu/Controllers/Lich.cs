#region

using Api.Areas.Edu.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Api.Areas.Edu.Controllers;

/// <summary>
/// </summary>
[Area("Api")]
[Route("/[area]/[controller]")]
[ApiController]
public class Lich : ControllerBase
{
	private readonly AppDbContext _database;

	public Lich(AppDbContext database)
	{
		_database = database;
	}

	/// <summary>
	/// </summary>
	/// <returns></returns>
	// GET: api/Lich
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Models.Lich>>> GetAll()
	{
		return await _database.Lich.ToListAsync(HttpContext.RequestAborted);
	}

	/// <summary>
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	// GET: api/Lich/5
	[HttpGet("{id}")]
	public async Task<ActionResult<Models.Lich>> Get(Guid id)
	{
		Models.Lich? lich = await _database.Lich.FindAsync(id, HttpContext.RequestAborted);

		return lich == null ? NotFound() : lich;
	}

	/// <summary>
	/// </summary>
	/// <param name="id"></param>
	/// <param name="lich"></param>
	/// <returns></returns>
	// PUT: api/Lich/5
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPut("{id}")]
	public async Task<IActionResult> PutLich(Guid id, Models.Lich lich)
	{
		if (id != lich.Id) return BadRequest();

		_database.Entry(lich).State = EntityState.Modified;

		try
		{
			await _database.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!LichExists(id))
				return NotFound();
			throw;
		}

		return NoContent();
	}

	/// <summary>
	/// </summary>
	/// <param name="lich"></param>
	/// <returns></returns>
	// POST: api/Liches
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPost]
	public async Task<ActionResult<Models.Lich>> Post(Models.Lich lich)
	{
		_database.Lich.Add(lich);
		await _database.SaveChangesAsync(HttpContext.RequestAborted);

		return CreatedAtAction("Get", new { id = lich.Id }, lich);
	}

	// DELETE: api/Lich/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		Models.Lich? lich = await _database.Lich.FindAsync(id);
		if (lich == null) return NotFound();

		_database.Lich.Remove(lich);
		await _database.SaveChangesAsync();

		return NoContent();
	}

	private bool LichExists(Guid id)
	{
		return _database.Lich.Any(e => e.Id == id);
	}
}