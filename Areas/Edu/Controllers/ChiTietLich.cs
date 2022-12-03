using Api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Api.Controllers;

[Area("Api")]
[ApiController]
[Route("[area]/ChiTietLich")]
public partial class ChiTietLich : ControllerBase
{
    private readonly AppDbContext _context;

    public ChiTietLich(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/ChiTietLich
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.ChiTietLich>>> GetAll()
    {
        return await _context.ChiTietLich.ToListAsync();
    }

    // GET: api/ChiTietLich/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.ChiTietLich>> Get(Guid id)
    {
        var chiTietLich = await _context.ChiTietLich.FindAsync(id);

        return chiTietLich == null ? NotFound() : chiTietLich;
    }

    // PUT: api/ChiTietLich/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Models.ChiTietLich chiTietLich)
    {
        if (id != chiTietLich.Id) return BadRequest();

        _context.Entry(chiTietLich).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ChiTietLichExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/ChiTietLich
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Models.ChiTietLich>> Post(Models.ChiTietLich chiTietLich)
    {
        _context.ChiTietLich.Attach(chiTietLich);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetAll", new { id = chiTietLich.Id }, chiTietLich);
    }

    // DELETE: api/ChiTietLich/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChiTietLich(Guid id)
    {
        var chiTietLich = await _context.ChiTietLich.FindAsync(id);
        if (chiTietLich == null) return NotFound();

        _context.ChiTietLich.Remove(chiTietLich);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ChiTietLichExists(Guid id)
    {
        return _context.ChiTietLich.Any(e => e.Id == id);
    }
}