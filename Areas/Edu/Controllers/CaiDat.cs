using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Api.Controllers;

[Area("Api")]
[ApiController]
[Route("/[area]/[controller]")]
public class CaiDat : ControllerBase
{
    private readonly Contexts.AppDbContext _context;

    public CaiDat(Contexts.AppDbContext context)
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
        Object[] objs = new Object[] { id };
        var caiDat = await _context.CaiDat.FindAsync(keyValues: objs, cancellationToken: HttpContext.RequestAborted);
        if (caiDat is null)
            return NotFound();
        return Ok(caiDat);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CaiDatDTO caiDat)
    {
        if (await _context.CaiDat.AnyAsync(x => x.Key == caiDat.Key, HttpContext.RequestAborted))
            return Conflict();
        _context.Add(caiDat as Models.CaiDat);
        await _context.SaveChangesAsync(HttpContext.RequestAborted);
        return Ok(caiDat);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] CaiDatDTO caiDat)
    {
        Models.CaiDat? caiDatMoi = await _context.CaiDat.Where(x => true).Take(1).FirstOrDefaultAsync(HttpContext.RequestAborted);
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
        return await _context.CaiDat.AnyAsync(x => x.Key == key, HttpContext.RequestAborted);
    }

    [NonAction]
    public async Task<bool> IsExists(Guid id)
    {
        return await _context.CaiDat.AnyAsync(x => x.Id == id, HttpContext.RequestAborted);
    }

    public class CaiDatDTO : Models.CaiDat
    {
    }
}