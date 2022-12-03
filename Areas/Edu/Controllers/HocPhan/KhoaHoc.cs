using Api.Contexts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Api.Areas.Api.Controllers;

[Area("Api")]
[Route("/[area]/KhoaHoc")]
[ApiController]
public class KhoaHoc : ControllerBase
{
    private readonly AppDbContext _database;

    public KhoaHoc(AppDbContext database)
    {
        _database = database;
    }

    // GET: api/KhoaHoc
    [HttpGet]
    public async Task<IActionResult> GetKhoaHoc()
    {
        return Ok(await _database.KhoaHoc.AsNoTracking().Select(x => new
        {
            x.Id,
            x.Ten,
            x.MieuTa
        }).ToListAsync());
    }

    // GET: api/KhoaHoc/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.KhoaHoc>> GetKhoaHoc(Guid id)
    {
        var khoaHoc = await _database.KhoaHoc.FindAsync(id);

        return khoaHoc == null ? NotFound() : khoaHoc;
    }

    // PUT: api/KhoaHoc/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutKhoaHoc(Guid id, Models.KhoaHoc khoaHoc)
    {
        if (id != khoaHoc.Id) return BadRequest();

        _database.Entry(khoaHoc).State = EntityState.Modified;

        try
        {
            await _database.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!KhoaHocExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/KhoaHoc
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [ProducesResponseType(typeof(Models.KhoaHoc), 201)]
    public async Task<ActionResult<Models.KhoaHoc>> PostKhoaHoc(Models.KhoaHoc khoaHoc)
    {
        await using IDbContextTransaction transaction = await _database.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            _database.KhoaHoc.Attach(khoaHoc);
            await _database.SaveChangesAsync();
            await transaction.CommitAsync(HttpContext.RequestAborted);
            return CreatedAtAction("GetKhoaHoc", new { id = khoaHoc.Id }, khoaHoc);
        }
        catch (Exception)
        {
            transaction.Rollback();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    // DELETE: api/KhoaHoc/5
    [HttpDelete]
    public async Task<IActionResult> DeleteKhoaHoc(Guid id)
    {
        var khoaHoc = await _database.KhoaHoc.FindAsync(id);
        if (khoaHoc == null) return NotFound();

        _database.KhoaHoc.Remove(khoaHoc);
        await _database.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch]
    [ProducesResponseType(typeof(Models.KhoaHoc), 200)]
    public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Models.KhoaHoc> path)
    {
        await using var transaction = _database.Database.BeginTransaction(IsolationLevel.Serializable);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            var khoaHoc = await _database.KhoaHoc.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
            if (khoaHoc is null)
                return NotFound();

            path.ApplyTo(khoaHoc, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _database.SaveChangesAsync(HttpContext.RequestAborted);
            transaction.Commit();

            return Ok(khoaHoc);
        }
        catch (Exception)
        {
            transaction.Rollback();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    private bool KhoaHocExists(Guid id)
    {
        return _database.KhoaHoc.Any(e => e.Id == id);
    }
}