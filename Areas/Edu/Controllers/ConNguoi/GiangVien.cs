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
public class GiangVien : ControllerBase
{
    private readonly AppDbContext _context;

    /// <summary>
    /// </summary>
    /// <param name="database"></param>
    public GiangVien(AppDbContext database)
    {
        _context = database;
    }

    [HttpGet("chitiet")]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var data = await (
                from x in _context.GiangVien
                where x.Id == id
                select new
                {
                    x.DonViCongTac,
                    x.TrinhDo
                }
            ).AsNoTracking().FirstOrDefaultAsync(HttpContext.RequestAborted);
        return (data is null) ? NotFound() : Ok(data);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.GiangVien.ToArrayAsync(HttpContext.RequestAborted));
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var giangVien = await _context.GiangVien.Include(x => x.SoYeuLyLich)
            .FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
        return giangVien is not null ? Ok(giangVien) : NotFound();
    }

    /// <summary>
    /// </summary>
    /// <param name="giangVien"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(Models.GiangVien giangVien)
    {
        using var transaction =
            await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
        try
        {
            await transaction.CreateSavepointAsync("TruocKhiTao", HttpContext.RequestAborted);
            if (await giangVien.ChuanBiThemAsync(_context, HttpContext.RequestAborted))
                return Conflict(new
                {
                    target = "TaiKhoan.TaiKhoanDangNhap",
                    message = "Tài khoản đăng nhập đã tồn tại"
                });

            _context.GiangVien.Attach(giangVien);
            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            await transaction.CommitAsync(HttpContext.RequestAborted);
            return CreatedAtAction(nameof(Get), new { id = giangVien.Id }, giangVien);
        }
        catch (Exception e)
        {
            transaction.RollbackToSavepoint("TruocKhiTao");
            return Conflict(e.InnerException?.Message ?? e.Message);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        Models.GiangVien giangVien = new() { Id = id };
        _context.Entry(giangVien).State = EntityState.Deleted;
        var changes = await _context.SaveChangesAsync(HttpContext.RequestAborted);
        return changes == 0 ? NotFound() : NoContent();
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="patch"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<IActionResult> Update(Guid id, [FromBody] JsonPatchDocument<Models.GiangVien> patch)
    {
        Models.GiangVien? giangVien = await _context.GiangVien
            .Where(x => x.Id == id)
            .Include(x => x.SoYeuLyLich)
            .ThenInclude(x => x!.ChoOHienNay)
            .ThenInclude(x => x!.Tinh)
            .Include(x => x.SoYeuLyLich!.ChoOHienNay!.QuanHuyen)
            .FirstOrDefaultAsync(HttpContext.RequestAborted);

        if (giangVien is null) return NotFound();
        using IDbContextTransaction transaction =
            await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("1", HttpContext.RequestAborted);
        patch.ApplyTo(giangVien, ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            transaction.Commit();
            return Ok(giangVien);
        }
        catch (Exception)
        {
            transaction.RollbackToSavepoint("1");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}