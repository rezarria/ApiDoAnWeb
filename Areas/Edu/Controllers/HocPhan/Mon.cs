using Api.Contexts;
using Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swashbuckle.AspNetCore.Filters;
using System.Data;

namespace Api.Areas.Api.Controllers;

/// <summary>
///     RESTful API của môn
/// </summary>
[Area("Api")]
[ApiController]
[Route("/[area]/[controller]")]
public class MonController : ControllerBase
{
    /// <summary>
    /// </summary>
    private readonly AppDbContext _database;

    /// <summary>
    /// </summary>
    /// <param name="database"></param>
    public MonController(AppDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int take = -1, [FromQuery] int skip = 0)
    {
        var data = take == -1 ? _database.Mon.Skip(skip).AsNoTracking() : _database.Mon.Skip(skip).Take(take).AsNoTracking();
        return Ok(await data.Select(x => new
        {
            x.Id,
            x.Ten,
            x.MieuTa,
        }).ToArrayAsync(HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Lấy môn theo id
    /// </summary>
    /// <param name="id">Mã GUID</param>
    /// <returns>Môn theo id</returns>
    /// <response code="200">Khi tìm thấy</response>
    /// <response code="400">Khi không tìm thấy</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Mon), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _database.Mon.SingleAsync(x => x.Id == id, HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Tạo môn mới
    /// </summary>
    /// <param name="mon"></param>
    /// <returns></returns>
    /// <response code="200">Tạo môn mới thành công và trả về môn</response>
    [HttpPost]
    [ProducesResponseType(typeof(Mon), 201)]
    public async Task<IActionResult> Post([FromBody] Models.DTO.MonDTO mon)
    {
        await using IDbContextTransaction transaction = await _database.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            Mon monMoi = new()
            {
                Ten = mon.Ten,
                MieuTa = mon.MieuTa,
                ThoiGianTao = DateTime.Now
            };

            if (mon.DanhMucMon is not null)
                monMoi.DanhMucMon = (from x in mon.DanhMucMon select new Models.DanhMucMon() { Id = x.Id }).ToList();

            if (mon.KhoaHoc is not null)
                monMoi.KhoaHoc = (from x in mon.KhoaHoc select new Models.KhoaHoc() { Id = x.Id }).ToList();

            _database.Attach(monMoi);
            await _database.SaveChangesAsync(HttpContext.RequestAborted);
            await transaction.CommitAsync(HttpContext.RequestAborted);

            return CreatedAtAction(nameof(Get), new { id = monMoi.Id }, monMoi);
        }
        catch (Exception)
        {
            transaction.Rollback();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    ///     Cập nhật môn theo id
    /// </summary>
    /// <param name="id">Guid</param>
    /// <param name="path">theo cấu trúc fast joson patch</param>
    /// <returns></returns>
    /// <response code="200">Cập nhật môn mới thành công và trả về môn</response>
    [HttpPatch]
    [ProducesResponseType(typeof(Mon), 200)]
    public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Mon> path)
    {
        await using IDbContextTransaction transaction = await _database.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            var mon = await _database.Mon.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
            if (mon is null)
                return NotFound();

            path.ApplyTo(mon, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _database.SaveChangesAsync(HttpContext.RequestAborted);
            transaction.Commit();

            return Ok(mon);
        }
        catch (Exception)
        {
            transaction.Rollback();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    ///     Xóa môn
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="402">Xóa thành công môn</response>
    /// <response code="400">không tìm thấy môn</response>
    /// <response code="500">Lỗi</response>
    [HttpDelete]
    [ProducesResponseType(402)]
    [ProducesResponseType(500)]
    [SwaggerResponseHeader(400, "", "", "")]
    public async Task<IActionResult> Delete(Guid id)
    {
        IDbContextTransaction transaction = await _database.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            var mon = _database.Mon.FirstOrDefault(x => x.Id == id);
            if (mon is null) return NotFound();
            _database.Entry(mon).State = EntityState.Deleted;
            await _database.SaveChangesAsync(HttpContext.RequestAborted);
            await transaction.CommitAsync(HttpContext.RequestAborted);
            return Ok();
        }
        catch (Exception)
        {
            return new StatusCodeResult(500);
        }
    }
}
