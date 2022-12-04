using Api.Contexts;
using Api.PhuTro;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Api.Areas.Api.Controllers;

/// <summary>
/// </summary>
[Area("Api")]
[ApiController]
[Route("/[area]/[controller]")]
public class QuanTri : ControllerBase
{
    private readonly AppDbContext _context;

    /// <summary>
    /// </summary>
    /// <param name="database"></param>
    public QuanTri(AppDbContext database)
    {
        _context = database;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _context.QuanTri
                                    .AsNoTracking()
                                    .ToArrayAsync(HttpContext.RequestAborted);
        return Ok(data);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var quanTri = await _context.QuanTri.Include(x => x.SoYeuLyLich)
            .FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
        return quanTri is not null ? Ok(quanTri) : NotFound();
    }

    /// <summary>
    /// </summary>
    /// <param name="quanTri"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Models.QuanTri quanTri)
    {
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            await transaction.CreateSavepointAsync("TruocKhiTao", HttpContext.RequestAborted);
            if (await quanTri.ChuanBiThemAsync(_context, HttpContext.RequestAborted))
                return Conflict(new
                {
                    target = "TaiKhoan.TaiKhoanDangNhap",
                    message = "Tài khoản đăng nhập đã tồn tại"
                });

            _context.QuanTri.Attach(quanTri);
            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            transaction.Commit();

            return CreatedAtAction(nameof(Get), new { id = quanTri.Id }, quanTri);
        }
        catch (Exception)
        {
            transaction.RollbackToSavepoint("TruocKhiTao");
            return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        Models.QuanTri quanTri = new() { Id = id };
        _context.Entry(quanTri).State = EntityState.Deleted;
        var changes = await _context.SaveChangesAsync(HttpContext.RequestAborted);
        return changes == 0 ? NotFound() : NoContent();
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="patch"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] JsonPatchDocument<Models.QuanTri> patch)
    {
        Models.QuanTri? quanTri = await _context.QuanTri
        .Where(x => x.Id == id)
        .Include(x => x.SoYeuLyLich)
            .ThenInclude(x => x!.ChoOHienNay)
            .ThenInclude(x => x!.Tinh)
        .Include(x => x.SoYeuLyLich!.ChoOHienNay!.QuanHuyen)
        .FirstOrDefaultAsync(HttpContext.RequestAborted);

        if (quanTri is null) return NotFound();
        using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("1", HttpContext.RequestAborted);
        patch.ApplyTo(quanTri, ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            transaction.Commit();
            return Ok(quanTri);
        }
        catch (Exception)
        {
            transaction.RollbackToSavepoint("1");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}