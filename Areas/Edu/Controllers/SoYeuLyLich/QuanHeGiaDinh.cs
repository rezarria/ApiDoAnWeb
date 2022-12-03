using Api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Api.Controllers;

/// <summary>
/// </summary>
[Area("Api")]
[Route("/[area]/[controller]")]
[ApiController]
public class QuanHeGiaDinh : ControllerBase
{
    private readonly AppDbContext _database;

    /// <summary>
    /// </summary>
    /// <param name="database"></param>
    public QuanHeGiaDinh(AppDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    // GET: api/QuanHeGiaDinhs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.SoYeuLyLich.QuanHeGiaDinh>>> GetQuanHeGiaDinh()
    {
        return await _database.QuanHeGiaDinh.ToListAsync(HttpContext.RequestAborted);
    }

    /// <summary>
    ///     Lấy danh sách quan hệ gia đình
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/QuanHeGiaDinhs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.SoYeuLyLich.QuanHeGiaDinh>> GetQuanHeGiaDinh(Guid id)
    {
        var quanHeGiaDinh = await _database.QuanHeGiaDinh.FindAsync(id, HttpContext.RequestAborted);

        return quanHeGiaDinh == null ? NotFound() : quanHeGiaDinh;
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="quanHeGiaDinh"></param>
    /// <returns></returns>
    // PUT: api/QuanHeGiaDinhs/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutQuanHeGiaDinh(Guid id, Models.SoYeuLyLich.QuanHeGiaDinh quanHeGiaDinh)
    {
        if (id != quanHeGiaDinh.Id) return BadRequest();

        _database.Entry(quanHeGiaDinh).State = EntityState.Modified;

        try
        {
            await _database.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QuanHeGiaDinhExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// </summary>
    /// <param name="quanHeGiaDinh"></param>
    /// <returns></returns>
    // POST: api/QuanHeGiaDinhs
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Models.SoYeuLyLich.QuanHeGiaDinh>> PostQuanHeGiaDinh(
        Models.SoYeuLyLich.QuanHeGiaDinh quanHeGiaDinh)
    {
        _database.QuanHeGiaDinh.Add(quanHeGiaDinh);
        await _database.SaveChangesAsync();

        return CreatedAtAction("GetQuanHeGiaDinh", new { id = quanHeGiaDinh.Id }, quanHeGiaDinh);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // DELETE: api/QuanHeGiaDinhs/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuanHeGiaDinh(Guid id)
    {
        var quanHeGiaDinh = await _database.QuanHeGiaDinh.FindAsync(id);
        if (quanHeGiaDinh == null) return NotFound();

        _database.QuanHeGiaDinh.Remove(quanHeGiaDinh);
        await _database.SaveChangesAsync();

        return NoContent();
    }

    private bool QuanHeGiaDinhExists(Guid id)
    {
        return _database.QuanHeGiaDinh.Any(e => e.Id == id);
    }
}