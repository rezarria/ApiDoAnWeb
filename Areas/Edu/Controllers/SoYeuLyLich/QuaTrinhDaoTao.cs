using Api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Api.Controllers;

/// <summary>
/// </summary>
[Area("Api")]
[Route("/[area]/[controller]")]
[ApiController]
public class QuaTrinhDaoTao : ControllerBase
{
    private readonly AppDbContext _database;

    /// <summary>
    /// </summary>
    /// <param name="database"></param>
    public QuaTrinhDaoTao(AppDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    // GET: api/QuaTrinhDaoTao
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.SoYeuLyLich.QuaTrinhDaoTao>>> GetMany()
    {
        return await _database.QuaTrinhDaoTao.ToListAsync(HttpContext.RequestAborted);
    }

    /// <summary>
    /// </summary>
    /// <param name="id">Guid của quá trình đào tạo</param>
    /// <returns></returns>
    // GET: api/QuaTrinhDaoTao/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.SoYeuLyLich.QuaTrinhDaoTao>> Get(Guid id)
    {
        var quaTrinhDaoTao = await _database.QuaTrinhDaoTao.FindAsync(id, HttpContext.RequestAborted);

        return quaTrinhDaoTao == null ? NotFound() : quaTrinhDaoTao;
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="quaTrinhDaoTao"></param>
    /// <returns></returns>
    // PUT: api/QuaTrinhDaoTao/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Models.SoYeuLyLich.QuaTrinhDaoTao quaTrinhDaoTao)
    {
        if (id != quaTrinhDaoTao.Id) return BadRequest();

        _database.Entry(quaTrinhDaoTao).State = EntityState.Modified;

        try
        {
            await _database.SaveChangesAsync(HttpContext.RequestAborted);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QuaTrinhDaoTaoExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// </summary>
    /// <param name="quaTrinhDaoTao"></param>
    /// <returns></returns>
    // POST: api/QuaTrinhDaoTaos
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Models.SoYeuLyLich.QuaTrinhDaoTao>> Post(
        Models.SoYeuLyLich.QuaTrinhDaoTao quaTrinhDaoTao)
    {
        _database.QuaTrinhDaoTao.Add(quaTrinhDaoTao);
        await _database.SaveChangesAsync(HttpContext.RequestAborted);

        return Ok(quaTrinhDaoTao);
    }

    /// <summary>
    /// </summary>
    /// <param name="id">Guid của quá trình đoàn tạo</param>
    /// <returns></returns>
    // DELETE: api/QuaTrinhDaoTao/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var quaTrinhDaoTao = await _database.QuaTrinhDaoTao.FindAsync(id, HttpContext.RequestAborted);
        if (quaTrinhDaoTao == null) return NotFound();

        _database.QuaTrinhDaoTao.Remove(quaTrinhDaoTao);
        await _database.SaveChangesAsync(HttpContext.RequestAborted);

        return NoContent();
    }

    private bool QuaTrinhDaoTaoExists(Guid id)
    {
        return _database.QuaTrinhDaoTao.Any(e => e.Id == id);
    }
}