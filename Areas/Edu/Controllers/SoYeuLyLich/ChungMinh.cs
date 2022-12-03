using Api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Api.Controllers;

/// <summary>
/// </summary>
[Area("api")]
[Route("api/[controller]")]
[ApiController]
public class ChungMinh : ControllerBase
{
    private readonly AppDbContext _database;

    /// <summary>
    /// </summary>
    /// <param name="database"></param>
    public ChungMinh(AppDbContext database)
    {
        _database = database;
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    // GET: api/ChungMinh
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.SoYeuLyLich.ChungMinh>>> GetChungMinh()
    {
        return await _database.ChungMinh.ToListAsync(HttpContext.RequestAborted);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/ChungMinhs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.SoYeuLyLich.ChungMinh>> GetChungMinh(Guid id)
    {
        var chungMinh = await _database.ChungMinh.FindAsync(id);

        return chungMinh == null ? NotFound() : chungMinh;
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="chungMinh"></param>
    /// <returns></returns>
    // PUT: api/ChungMinhs/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutChungMinh(Guid id, Models.SoYeuLyLich.ChungMinh chungMinh)
    {
        if (id != chungMinh.Id) return BadRequest();

        _database.Entry(chungMinh).State = EntityState.Modified;

        try
        {
            await _database.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ChungMinhExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// </summary>
    /// <param name="chungMinh"></param>
    /// <returns></returns>
    // POST: api/ChungMinhs
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Models.SoYeuLyLich.ChungMinh>> PostChungMinh(Models.SoYeuLyLich.ChungMinh chungMinh)
    {
        _database.ChungMinh.Add(chungMinh);
        await _database.SaveChangesAsync();

        return CreatedAtAction("GetChungMinh", new { id = chungMinh.Id }, chungMinh);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // DELETE: api/ChungMinhs/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChungMinh(Guid id)
    {
        var chungMinh = await _database.ChungMinh.FindAsync(id);
        if (chungMinh == null) return NotFound();

        _database.ChungMinh.Remove(chungMinh);
        await _database.SaveChangesAsync();

        return NoContent();
    }

    private bool ChungMinhExists(Guid id)
    {
        return _database.ChungMinh.Any(e => e.Id == id);
    }
}