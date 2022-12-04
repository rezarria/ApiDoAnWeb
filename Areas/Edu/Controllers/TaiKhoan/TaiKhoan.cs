using Api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Api.Controllers;

[Area("Api")]
[Route("[area]/[controller]")]
[ApiController]
public partial class TaiKhoan
{
    private readonly AppDbContext _context;

    public TaiKhoan(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/TaiKhoan
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.TaiKhoan>>> GetTaiKhoan()
    {
        return await _context.TaiKhoan.ToListAsync();
    }

    // GET: api/TaiKhoan/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.TaiKhoan>> GetTaiKhoan(Guid id)
    {
        var taiKhoan = await _context.TaiKhoan.FindAsync(id);

        return taiKhoan == null ? NotFound() : taiKhoan;
    }

    // PUT: api/TaiKhoan/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTaiKhoan(Guid id, Models.TaiKhoan taiKhoan)
    {
        if (id != taiKhoan.Id) return BadRequest();

        _context.Entry(taiKhoan).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaiKhoanExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/TaiKhoan
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Models.TaiKhoan>> PostTaiKhoan(Models.TaiKhoan taiKhoan)
    {
        _context.TaiKhoan.Add(taiKhoan);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTaiKhoan", new { id = taiKhoan.Id }, taiKhoan);
    }

    // DELETE: api/TaiKhoan/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaiKhoan(Guid id)
    {
        var taiKhoan = await _context.TaiKhoan.FindAsync(id);
        if (taiKhoan == null) return NotFound();

        _context.TaiKhoan.Remove(taiKhoan);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TaiKhoanExists(Guid id)
    {
        return _context.TaiKhoan.Any(e => e.Id == id);
    }
}