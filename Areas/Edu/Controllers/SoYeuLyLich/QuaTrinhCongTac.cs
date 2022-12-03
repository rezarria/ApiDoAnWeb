using Api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Api.Controllers;

/// <summary>
/// </summary>
[Area("Api")]
[Route("/[area]/[controller]")]
[ApiController]
public class QuaTrinhCongTac : ControllerBase
{
    private readonly AppDbContext database;

    /// <summary>
    /// </summary>
    /// <param name="database"></param>
    public QuaTrinhCongTac(AppDbContext database)
    {
        this.database = database;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    // GET: api/QuaTrinhCongTacs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.SoYeuLyLich.QuaTrinhCongTac>>> GetAll()
    {
        return await database.QuaTrinhCongTac.ToListAsync(HttpContext.RequestAborted);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/QuaTrinhCongTac/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.SoYeuLyLich.QuaTrinhCongTac>> Get(Guid id)
    {
        var quaTrinhCongTac = await database.QuaTrinhCongTac.FindAsync(id, HttpContext.RequestAborted);

        return quaTrinhCongTac == null ? NotFound() : quaTrinhCongTac;
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="quaTrinhCongTac"></param>
    /// <returns></returns>
    // PUT: api/QuaTrinhCongTac/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Models.SoYeuLyLich.QuaTrinhCongTac quaTrinhCongTac)
    {
        if (id != quaTrinhCongTac.Id) return BadRequest();

        database.Entry(quaTrinhCongTac).State = EntityState.Modified;

        try
        {
            await database.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QuaTrinhCongTacExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// </summary>
    /// <param name="quaTrinhCongTac"></param>
    /// <returns></returns>
    // POST: api/QuaTrinhCongTacs
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Models.SoYeuLyLich.QuaTrinhCongTac>> Post(
        Models.SoYeuLyLich.QuaTrinhCongTac quaTrinhCongTac)
    {
        database.QuaTrinhCongTac.Add(quaTrinhCongTac);
        await database.SaveChangesAsync(HttpContext.RequestAborted);

        return Ok(quaTrinhCongTac);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // DELETE: api/QuaTrinhCongTacs/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var quaTrinhCongTac = await database.QuaTrinhCongTac.FindAsync(id, HttpContext.RequestAborted);
        if (quaTrinhCongTac == null) return NotFound();

        database.QuaTrinhCongTac.Remove(quaTrinhCongTac);
        await database.SaveChangesAsync(HttpContext.RequestAborted);

        return NoContent();
    }

    private bool QuaTrinhCongTacExists(Guid id)
    {
        return database.QuaTrinhCongTac.Any(e => e.Id == id);
    }
}